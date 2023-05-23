using BookStore.Models;
using BookStore.Services.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class UserService
    {
        private readonly IMongoCollection<UserRegistrationModel> _userRegistration;
        private readonly IMongoCollection<LoginModel> _login;
        private readonly IMongoCollection<UserProfileModel> _userProfile;
        private readonly IMongoCollection<Order> _order;


        public UserService(IDataConnection dataConnection)
        {
            var client = new MongoClient(dataConnection.ConnectionString);
            var database = client.GetDatabase(dataConnection.DatabaseName);

            _userRegistration = database.GetCollection<UserRegistrationModel>("UserRegistration");
            _login = database.GetCollection<LoginModel>("Login");
            _userProfile = database.GetCollection<UserProfileModel>("UserProfile");
            _order = database.GetCollection<Order>("Order");
        }

        public UserRegistrationModel findMember(string email)
        {
            try
            {
                var filter = Builders<UserRegistrationModel>.Filter.Eq(x => x.Email, email);
                var user = _userRegistration.Find(filter).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in findMember" + ex.Message);
            }
        }

        public UserRegistrationModel RegisterUser(UserRegistrationModel userRegistrationModel, LoginModel loginModel, UserProfileModel userProfileModel)
        {
            try
            {
                var userExist = _userRegistration.Find(reg => reg.Email == userRegistrationModel.Email);

                if (userExist.CountDocuments() == 0)
                {
                    _login.InsertOne(loginModel);
                    _userRegistration.InsertOne(userRegistrationModel);
                    _userProfile.InsertOne(userProfileModel);
                }
                return userRegistrationModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in RegisterUser" + ex.Message);
            }
        }
       
        public bool updateValidStatus(UserRegistrationModel userRegModel, LoginModel LoginModel)
        {
            try
            {

                var filter = Builders<UserRegistrationModel>.Filter.Eq(x => x.Email, userRegModel.Email);
                var update = Builders<UserRegistrationModel>.Update.Set("Valid Status", userRegModel.ValidStatus);
                var filter_log = Builders<LoginModel>.Filter.Eq(x => x.Email, LoginModel.Email);
                var update_log = Builders<LoginModel>.Update.Set("Valid Status", LoginModel.ValidStatus);

                _userRegistration.UpdateOne(filter, update);
                _login.UpdateOne(filter_log, update_log);

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error in findMember" + ex.Message);
            }
        }

        public LoginModel validateCredential(LoginModel loginModel)
        {
            try
            {
                var filter = Builders<LoginModel>.Filter.Eq(x => x.Email, loginModel.Email);
                var result = _login.Find(filter).FirstOrDefault();
                if (result != null)
                {
                    if (Common.Base64.Base64Decode(result.Password) == loginModel.Password)
                        result.LoginStatus = true;
                    else
                        result.LoginStatus = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in validateCredential" + ex.Message);
            }
        }

        public void InsertOrder(Order order)
        {
            try
            {
                _order.InsertOne(order);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in insertOrder" + ex.Message);
            }

        }

        public List<Order> UserOrder(string userId)
        {
            var filter = Builders<Order>.Filter.Eq("items.UserId", userId);

            // Execute the query and retrieve the matching orders
            List<Order> userOrders = _order.Find(filter).ToList();
            return userOrders;

        }
    }
}
