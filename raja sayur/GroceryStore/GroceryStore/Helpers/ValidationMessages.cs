using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStore.Helpers
{
    public class ValidationMessages
    {
        // Register 
        public static string NameRequired = "Name is required";
        public static string NameLetters = "Name only accept letters";
        public static string EmailRequired = "Email is required";
        public static string PhoneNumberRequired = "Phone Number is required";
        public static string PhoneNumberOnly = "Phone Number only accept numbers";
        public static string PasswordRequired = "Password is required";
        public static string PasswordMinimum = "Minimum 6 characters required for Password";
        public static string PhoneNumberMinimum = "Minimum 10 digits required for Phone Number";
        public static string PhoneNumberMaximum = "Maximum 13 digits required for  Phone Number";
        public static string TermsAccept = "Please accept terms and conditions";
        public static string TotalFixed = "Minimum order amount should be Rp. ";

        // Add Address 
        public static string HouseNoRequired = "House Number is required";
        public static string CountryRequired = "Country is required";
        public static string StateRequired = "State is required";
        public static string ApartmentRequired = "Apartment is required";
        public static string StreetDetailRequired = "Street is required";
        public static string AreaDetailRequired = "Area is required";
        public static string CityRequired = "City is required";
        public static string PincodeRequired = "Pincode is required";
        public static string PincodeMinimum = "Minimum 5 digits required for Pincode";

        // Change Password
        public static string CurrentPasswordRequired = "Current password is required";
        public static string NewPasswordRequired = "New password is required";
        public static string RepeatPasswordRequired = "Confirm password is required";
        public static string ConfirmPasswordRequired = "Confirm password is required";
        public static string PasswordMatch = "Password does not match with Confirm password";
        public static string ConfirmPasswordMatch = "Password does not match with Confirm password";
        public static string NewPasswordMinimum = "Minimum 6 characters required for New Password";
        //
        public static string ReScheduleDate = "Please select Re-Schedule Date";

        // Forgot Password
        public static string MobileNumberRequired = "Mobile number is required";

        // Resend OTP
        public static string OTPResendSuccess = "OTP sent to your mobile number";
        public static string OTPvalidate = "OTP did not match";
        // Resend OTP
        public static string SearchTextRequired = "Search field is required";

        // Empty Page Messages
        public static string EmptyCart = "Empty Cart";
        public static string EmptyNotifications = "Empty notifications";
        public static string EmptyAddress = "No address found";
        public static string EmptyFavouriteProducts = "No favourite products found";
        public static string EmptyProducts = "No products found";
        public static string EmptyHelp = "No help data";
        public static string EmptyCoupon = "No coupons found";
    }
}
