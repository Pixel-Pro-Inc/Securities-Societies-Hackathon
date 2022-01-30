using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ServiceController : BaseApiController
    {
        private readonly IFileService _fileService;

        public ServiceController(IFileService fileService)
        {
            _fileService = fileService;
        }

        #region GetApplications
        [Authorize]
        [HttpGet("getmyapplications/{accountId}")]
        public async Task<ActionResult<List<IDictionary<string, object>>>> GetApps(string accountId)
        {
            User user = await GetUser(accountId);

            List<NameAuth> result = await _firebaseDataContext.GetData<NameAuth>("NameAuthorisation");

            var myApps = result.Where(r => r.User.Email == user.Email || r.User.PhoneNumber == user.PhoneNumber).ToList();

            List<IDictionary<string, object>> keyValues = new List<IDictionary<string, object>>();

            foreach (var item in myApps)
            {
                IDictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add("corrections", item.Corrections);
                keyValuePairs.Add("rejected", item.Rejected);
                keyValuePairs.Add("accepted", item.Accepted);
                keyValuePairs.Add("review", item.Review);
                keyValuePairs.Add("type", item.SocietyType);
                keyValuePairs.Add("stage", "Name Authorisation");
                keyValuePairs.Add("feedback", item.Feedback);
                keyValuePairs.Add("Id", item.Id);

                keyValues.Add(keyValuePairs);
            }

            return keyValues;
        }
        #endregion

        #region NameAuthorisation
        [Authorize]
        [HttpPost("nameauth/submit")]
        public async Task<ActionResult<NameAuthDto>> NameAuthorisation(NameAuthDto nameAuthDto)
        {
            NameAuth nameAuth = new NameAuth()
            {
                Names = await ConvertToList(nameAuthDto.societyNames),
                Objectives = await ConvertToList(nameAuthDto.objectives),
                SocietyType = nameAuthDto.societyType,
                User = await GetUser(nameAuthDto.accountId),
                Review = true,
                DateSubmitted = DateTime.Now
            };

            nameAuth.Id = await GetId();

            nameAuth.FileUrls = new List<string>();

            foreach (var item in nameAuthDto.files)
            {
                string[] vals = await GetNameAndData(item);

                var result = await _fileService.Submit(vals[0].Trim(), vals[1].Trim());

                if (result != null)
                    nameAuth.FileUrls.Add(result);
            }

            _firebaseDataContext.StoreData("NameAuthorisation/" + nameAuth.Id, nameAuth);

            return nameAuthDto;
        }

        private async Task<string[]> GetNameAndData(string input)
        {
            string[] vals = new string[2];

            foreach (char item in input)
            {
                if(item == ',')
                {
                    vals[1] = input.Substring(0, input.IndexOf(item));
                    vals[0] = input.Substring(input.IndexOf(item) + 1, input.Length - (input.IndexOf(item) + 1));

                    break;
                }
            }

            return vals;
        }

        private async Task<List<string>> ConvertToList(string input)
        {
            string[] stringsArray = input.Split(',');
            List<string> stringsList = new List<string>(stringsArray.Length);
            stringsList.AddRange(stringsArray);

            List<string> result = new List<string>();

            foreach (string item in stringsList)
            {
                result.Add(item.Trim());
            }

            return result;
        }
        /*
         The same code here is being defined in BaseAPIController, so noone will miss it here
          private async Task<User> GetUser(string accountID)
        {
            List<User> users = await _firebaseDataContext.GetData<User>("Account");
            return users.Where(u => u.Email == accountID || u.PhoneNumber.ToString() == accountID).ToList().Count != 0 ? users.Where(u => u.Email == accountID || u.PhoneNumber.ToString() == accountID).ToList()[0] : null;
        }
         */
       

        async Task<int> GetId()
        {
            List<NameAuth> nameAuths = await _firebaseDataContext.GetData<NameAuth>("NameAuthorisation");

            int lastId = -1;

            if (nameAuths.Count != 0)
                lastId = nameAuths.Where(u => u.Id == (nameAuths.Count - 1)).ToList()[0].Id;

            return (lastId + 1);
        }
        #endregion
    }
}
