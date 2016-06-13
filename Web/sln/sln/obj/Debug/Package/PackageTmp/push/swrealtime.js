'use strict';

importScripts('scripts/indexdbwrapper.js');

var API_ENDPOINT = 'https://imaot.co.il/t/getNotify2.ashx?d=';
var KEY_VALUE_STORE_NAME = 'key-value-store';
debugger;
var idb;

// avoid opening idb until first call
function getIdb() {
    if (!idb) {
        idb = new IndexDBWrapper('key-value-store', 1, function (db) {
            db.createObjectStore(KEY_VALUE_STORE_NAME);
        });
    }
    return idb;
}

function showNotification(title, body, icon, data) {
    var notificationOptions = {
        body: body,
        icon: icon ? icon : 'images/touch/chrome-touch-icon-192x192.png',
        tag: 'simple-push-demo-notification',
        data: data
    };
    if (self.registration.showNotification) {
        return self.registration.showNotification(title, notificationOptions);
    } else {
        new Notification(title, notificationOptions);
    }
}

self.addEventListener('push', function (event) {
    debugger;
    event.waitUntil(
      self.registration.pushManager.getSubscription().then(function (subscription) {
          debugger;
          var endpointSections = subscription.endpoint.split('/');
          var subscriptionId = endpointSections[endpointSections.length - 1];
          var gurl = self.API_ENDPOINT + subscriptionId;
          fetch(gurl).then(function (response) {
              if (response.status !== 200) {
                  console.log('Looks like there was a problem. Status Code: ' +
                    response.status);
                  // Throw an error so the promise is rejected and catch() is executed
                  throw new Error();
              }

              // Examine the text in the response
              return response.json().then(function (data) {
                  debugger;

                  var title = "שרות הודעות";//data.query.Title;
                  var message = "";//data.query.Body + "=>" + data.query.DeviceId;
                  // Add this to the data of the notification
                  var urlToOpen = "http://5.100.251.87:4545/";// data.query.Url;

                  if (data.IsError) {
                      message = data.ErrDesc;
                  }
                  else {
                      title = data.Title;
                      message = data.Body;
                       urlToOpen = data.Url;
                  }
                  var icon = 'images/touch/chrome-touch-icon-192x192.png';
                  var notificationTag = 'simple-push-demo-notification';

                  
                  if (!Notification.prototype.hasOwnProperty('data')) {
                      // Since Chrome doesn't support data at the moment
                      // Store the URL in IndexDB
                      getIdb().put(KEY_VALUE_STORE_NAME, notificationTag, urlToOpen);
                  }

                  var notificationFilter = {
                      tag: 'simple-push-notification'
                  };

                  var notificationData = {
                      url: urlToOpen
                  };

                  if (!self.registration.getNotifications) {
                      return showNotification(title, message, icon, notificationData);
                  }

                  return self.registration.getNotifications(notificationFilter)
                    .then(function (notifications) {
                        if (notifications && notifications.length > 0) {
                            // Start with one to account for the new notification
                            // we are adding
                            var notificationCount = 1;
                            for (var i = 0; i < notifications.length; i++) {
                                var existingNotification = notifications[i];
                                if (existingNotification.data &&
                                  existingNotification.data.notificationCount) {
                                    notificationCount +=
                                      existingNotification.data.notificationCount;
                                } else {
                                    notificationCount++;
                                }
                                existingNotification.close();
                            }
                            message = 'You have ' + notificationCount + '  updates.';
                            notificationData.notificationCount = notificationCount;
                        }
                        return showNotification(title, message, icon, notificationData);
                    });
              });
          })

      })
    );
});

self.addEventListener('notificationclick', function (event) {
    debugger;
    var url = event.notification.data.url;
    event.notification.close();
    event.waitUntil(clients.openWindow(url));
});
