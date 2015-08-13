using Michal.Project.Contract;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Michal.Project.Models
{
    public class ViewItem
    {
        public int StatusId { get; set; }
        public string StatusDesc { get; set; }
        public string FieldShowMy { get; set; }

        public Expression<Func<Shipping, bool>> GetMyRecords(IUserContext user)
        {
            ParameterExpression shExp = Expression.Parameter(typeof(Shipping), "Shipping");
            Expression showMy = Expression.Equal(Expression.Property(shExp, this.FieldShowMy), Expression.Constant(user.UserId, typeof(Guid?)));
            var NoNull = Expression.NotEqual(Expression.Property(shExp, this.FieldShowMy), Expression.Constant(null, typeof(Guid?)));
            Expression andExpression = Expression.AndAlso(NoNull, showMy);
            Expression<Func<Shipping, bool>> showOnlyMyRecord = Expression.Lambda<Func<Shipping, bool>>(andExpression, shExp);
            return showOnlyMyRecord;
        }
     

        //public bool IsVisbleForAdmin { get; set; }
        //public bool IsVisbleForOrgManager { get; set; }
        //public bool IsVisbleForAccept { get; set; }
        //public bool IsVisbleForUser { get; set; }
        //public bool IsVisbleForRunner { get; set; }
    }
}