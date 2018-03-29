using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using RentACar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static RentACar.Models.ApplicationWideData;

[assembly: OwinStartupAttribute(typeof(RentACar.Startup))]
namespace RentACar
{
    public partial class Startup
    {
        private Entities db = new Entities();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AddDefaultRoleAndUser();
            AddDefaultVehicleTransmissions();
            AddDefaultRequestStatus();
            AddDefaultKeys();
            InitializeApplicationWideData();
        }

        private void AddDefaultRoleAndUser()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            
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
            var transmissionExists = db.VehicleTransmissions.Any(t => t.Code == ApplicationWideData.TransmissionAutoCode);
            if(!transmissionExists)
            {
                var transmissions = new List<VehicleTransmission>
                {
                    new VehicleTransmission
                    {
                        Code = TransmissionAutoCode,
                        Title = GetTranmissionString(TransmissionAutoCode)
                    },
                    new VehicleTransmission
                    {
                        Code = TranmissionManualCode,
                        Title = GetTranmissionString(TranmissionManualCode)
                    }
                };
                transmissions.ForEach(t => db.VehicleTransmissions.Add(t));
                db.SaveChanges();
            }
        }

        private void AddDefaultRequestStatus()
        {
            var db = new Entities();
            var requestStatusExists = db.RequestStatus.Any(rs => rs.Code == RequestStatusApproved);
            if(!requestStatusExists)
            {
                var requestStata = new List<RequestStatu>
                {
                    new RequestStatu
                    {
                        Code = RequestStatusApproved,
                        Title = GetRequestStatusString(ApplicationWideData.RequestStatusApproved)
                    },
                    new RequestStatu
                    {
                        Code = RequestStatusCancelled,
                        Title = GetRequestStatusString(RequestStatusCancelled)
                    },
                    new RequestStatu
                    {
                        Code = RequestStatusNotViewed,
                        Title = GetRequestStatusString(RequestStatusNotViewed)
                    }
                };
                requestStata.ForEach(rs => db.RequestStatus.Add(rs));
                db.SaveChanges();
            }

            if(!db.RequestStatus.Any(rs => rs.Code == RequestStatusNoAction))
            {
                var requestStatus = new RequestStatu
                {
                    Code = RequestStatusNoAction,
                    Title = GetRequestStatusString(RequestStatusNoAction)
                };
                db.RequestStatus.Add(requestStatus);
                db.SaveChanges();
            }
        }

        private void AddDefaultKeys()
        {
            try
            {
                var driverCost = db.GlobalDatas.Where(gd => gd.Key.Equals(DriverCostKey, StringComparison.OrdinalIgnoreCase)).First();
            }
            catch(InvalidOperationException)
            {
                var driverCost = new GlobalData()
                {
                    Key = DriverCostKey,
                    Value = "3500"
                };
                db.GlobalDatas.Add(driverCost);
                db.SaveChanges();
            }
            try
            {
                var defaultImg = db.GlobalDatas.Where(gd => gd.Key.Equals(DefaultImageKey, StringComparison.OrdinalIgnoreCase)).First();
            }
            catch (InvalidOperationException)
            {
                var defaultImg = new GlobalData()
                {
                    Key = DefaultImageKey,
                    Value = DefaultImageValue
                };
                db.GlobalDatas.Add(defaultImg);
                db.SaveChanges();
            }
            try
            {
                var fbkey = db.GlobalDatas.Where(gd => gd.Key.Equals(FaceBookKey, StringComparison.OrdinalIgnoreCase)).First();
            }
            catch (InvalidOperationException)
            {
                var fbkey = new GlobalData()
                {
                    Key = FaceBookKey,
                    Value = "https://www.facebook.com/speegotours"
                };
                db.GlobalDatas.Add(fbkey);
                db.SaveChanges();
            }
            try
            {
                var phone = db.GlobalDatas.Where(gd => gd.Key.Equals(PhoneNumberKey1, StringComparison.OrdinalIgnoreCase)).First();
            }
            catch (InvalidOperationException)
            {
                var phone = new GlobalData()
                {
                    Key = PhoneNumberKey1,
                    Value = "+92 331 6989 422"
                };
                db.GlobalDatas.Add(phone);
                db.SaveChanges();
            }
            try
            {
                var email = db.GlobalDatas.Where(gd => gd.Key.Equals(EmailKey1, StringComparison.OrdinalIgnoreCase)).First();
            }
            catch (InvalidOperationException)
            {
                var email = new GlobalData()
                {
                    Key = EmailKey1,
                    Value = "info@speegotours.com"
                };
                db.GlobalDatas.Add(email);
                db.SaveChanges();
            }
            try
            {
                var email = db.GlobalDatas.Where(gd => gd.Key.Equals(EmailKey1, StringComparison.OrdinalIgnoreCase)).First();
            }
            catch (InvalidOperationException)
            {
                var email = new GlobalData()
                {
                    Key = EmailKey1,
                    Value = "info@speegotours.com"
                };
                db.GlobalDatas.Add(email);
                db.SaveChanges();
            }
            try
            {
                var youtube = db.GlobalDatas.Where(gd => gd.Key.Equals(YouTubeKey, StringComparison.OrdinalIgnoreCase)).First();
            }
            catch (InvalidOperationException)
            {
                var youtube = new GlobalData()
                {
                    Key = YouTubeKey,
                    Value = "https://youtu.be/ye_altohtho"
                };
                db.GlobalDatas.Add(youtube);
                db.SaveChanges();
            }
            try
            {
                var appName = db.GlobalDatas.Where(gd => gd.Key.Equals(ApplicationNameKey, StringComparison.OrdinalIgnoreCase)).First();
            }
            catch (InvalidOperationException)
            {
                var appName = new GlobalData()
                {
                    Key = ApplicationNameKey,
                    Value = "Speego Tours"
                };
                db.GlobalDatas.Add(appName);
                db.SaveChanges();
            }
        }

        private void InitializeApplicationWideData()
        {
            FaceBookValue = db.GlobalDatas.Where(gd => gd.Key.Equals(FaceBookKey, StringComparison.OrdinalIgnoreCase)).First().Value;
            YouTubeValue = db.GlobalDatas.Where(gd => gd.Key.Equals(YouTubeKey, StringComparison.OrdinalIgnoreCase)).First().Value;
            EmailValue1 = db.GlobalDatas.Where(gd => gd.Key.Equals(EmailKey1, StringComparison.OrdinalIgnoreCase)).First().Value;
            PhoneNumberValue1 = db.GlobalDatas.Where(gd => gd.Key.Equals(PhoneNumberKey1, StringComparison.OrdinalIgnoreCase)).First().Value;
            PhoneNumberValue1 = db.GlobalDatas.Where(gd => gd.Key.Equals(PhoneNumberKey1, StringComparison.OrdinalIgnoreCase)).First().Value;
            ApplicationNameValue = db.GlobalDatas.Where(gd => gd.Key.Equals(ApplicationNameKey, StringComparison.OrdinalIgnoreCase)).First().Value;
        }
    }
}
