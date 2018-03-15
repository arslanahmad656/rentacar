using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.Models
{
    public static class ApplicationWideData
    {
        #region Transmission Codes
        /// <summary>
        /// Gives the transmission code for Automatic in integer format
        /// </summary>
        public static int TransmissionAutoCode => 1;
        /// <summary>
        /// Gives the transmission code for Manual in integer format
        /// </summary>
        public static int TranmissionManualCode => 2;
        /// <summary>
        /// Returns the string format for the integer transmission code.
        /// </summary>
        /// <param name="code">
        /// The code (that must be either 1 or 2 respectively for auto and manual) whose string representation is required
        /// </param>
        /// <returns>
        /// The string representation of transmission code.
        /// Null if the transmission code is invalid.
        /// </returns>
        public static string GetTranmissionString(int code) => 
            code == 1 ? "Auto" : code == 2 ? "Manual" : null;
        #endregion

        #region RequestStatusCodes
        /// <summary>
        /// Gives the request status code for Approved in integer format
        /// </summary>
        public static int RequestStatusApproved => 1;
        /// <summary>
        /// Gives the request status code for Cancelled in integer format
        /// </summary>
        public static int RequestStatusCancelled => 2;
        /// <summary>
        /// Gives the request status code for Not Viewed in integer format
        /// </summary>
        public static int RequestStatusNotViewed => 3;
        /// <summary>
        /// Returns the string format for the integer request status code.
        /// </summary>
        /// <param name="code">
        /// The code (that must be either 1, 2 or 3 respectively for Approved, Cancelled or Not Viewed) whose string representation is required
        /// </param>
        /// <returns>
        /// The string representation of transmission code.
        /// Null if the transmission code is invalid.
        /// </returns>
        public static string GetRequestStatusString(int code) =>
            code == 1 ? "Approved" : code == 2 ? "Cancelled" : code == 3 ? "Not Viewed" : null;
        #endregion

        #region OtherCodes

        public static bool VisitorQueryViewed => true;

        public static bool VisitorQueryNotViewed => false;

        public static string GetVisitorQueryStatus(bool code) =>
            code ? "true" : "false";

        public static string StorageRootPath => "~/uploads";

        public static string DriverCostKey => "drivercost";

        public static int VehicleModelLowerLimt => 1960;

        public static int VehicleModelUpperLimt => DateTime.Now.Year;

        public static int FareDaysStep1 => 3;

        public static int FareDaysStep2 => 7;

        public static int FareDaysStep3 => 14;

        #endregion
    }
}