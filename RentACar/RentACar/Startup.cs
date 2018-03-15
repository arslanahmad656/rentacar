using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using RentACar.Models;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(RentACar.Startup))]
namespace RentACar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AddDefaultRoleAndUser();
            AddDefaultVehicleTransmissions();
            AddDefaultRequestStatus();
            AddDefaultDriverCost();
        }

        private void AddDefaultRoleAndUser()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            Entities db = new Entities();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var roleName = "admin";
            if(!roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole
                {
                    Name = roleName
                };
                var result = roleManager.Create(role);
                if(result.Succeeded)
                {
                    var user = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@admin.com"
                    };
                    var pwd = "Updating@1234";
                    result = userManager.Create(user, pwd);
                    if(result.Succeeded)
                    {
                        result = userManager.AddToRole(user.Id, roleName);
                        if(!result.Succeeded)
                        {
                            throw new System.Exception("Could not add admin to admin role");
                        }
                    }
                    else
                    {
                        throw new Exception("Could not create admin");
                    }
                }
                else
                {
                    throw new Exception("Could not create admin role");
                }
            }
        }

        private void AddDefaultVehicleTransmissions()
        {
            var db = new Entities();
            var transmissionExists = db.VehicleTransmissions.Any(t => t.Code == ApplicationWideData.TransmissionAutoCode);
            if(!transmissionExists)
            {
                var transmissions = new List<VehicleTransmission>
                {
                    new VehicleTransmission
                    {
                        Code = ApplicationWideData.TransmissionAutoCode,
                        Title = ApplicationWideData.GetTranmissionString(ApplicationWideData.TransmissionAutoCode)
                    },
                    new VehicleTransmission
                    {
                        Code = ApplicationWideData.TranmissionManualCode,
                        Title = ApplicationWideData.GetTranmissionString(ApplicationWideData.TranmissionManualCode)
                    }
                };
                transmissions.ForEach(t => db.VehicleTransmissions.Add(t));
                db.SaveChanges();
            }
        }

        private void AddDefaultRequestStatus()
        {
            var db = new Entities();
            var requestStatusExists = db.RequestStatus.Any(rs => rs.Code == ApplicationWideData.RequestStatusApproved);
            if(!requestStatusExists)
            {
                var requestStata = new List<RequestStatu>
                {
                    new RequestStatu
                    {
                        Code = ApplicationWideData.RequestStatusApproved,
                        Title = ApplicationWideData.GetRequestStatusString(ApplicationWideData.RequestStatusApproved)
                    },
                    new RequestStatu
                    {
                        Code = ApplicationWideData.RequestStatusCancelled,
                        Title = ApplicationWideData.GetRequestStatusString(ApplicationWideData.RequestStatusCancelled)
                    },
                    new RequestStatu
                    {
                        Code = ApplicationWideData.RequestStatusNotViewed,
                        Title = ApplicationWideData.GetRequestStatusString(ApplicationWideData.RequestStatusNotViewed)
                    }
                };
                requestStata.ForEach(rs => db.RequestStatus.Add(rs));
                db.SaveChanges();
            }
        }

        private void AddDefaultDriverCost()
        {
            var db = new Entities();
            try
            {
                var driverCost = db.GlobalDatas.Where(gd => gd.Key.Equals(ApplicationWideData.DriverCostKey, StringComparison.OrdinalIgnoreCase)).First();
            }
            catch(InvalidOperationException)
            {
                var driverCost = new GlobalData()
                {
                    Key = ApplicationWideData.DriverCostKey,
                    Value = "3500"
                };
                db.GlobalDatas.Add(driverCost);
                db.SaveChanges();
            }
        }
    }
}
