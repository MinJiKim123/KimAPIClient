using KimMinAPIClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KimMinAPIClient.Services
{
    public class HttpClientService
    {
        private HttpClient client;
        private HttpResponseMessage response;
        public HttpClientService(HttpClient htpcl)
        {
            client = htpcl;
            client.BaseAddress = new Uri("https://mkim168-eval-test.apigee.net/debproxy/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("apikey", "7yrlsUKsGJrm8qZekeG4dR3GgVwnnddK");
        }

        public async Task<IEnumerable<Expense>> GetExpensesAsync()
        {
            try
            {
                //get all items
                response = await client.GetAsync("api/expense");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    IEnumerable<Expense> items = JsonConvert.DeserializeObject<IEnumerable<Expense>>(json);
                    return items;
                }
               
            }catch(Exception e)
            {
                Console.WriteLine("error while getexpensesasync : " + e);
                return null;
            }
            return null;
        }
        public async Task<Expense> GetExpenseAsync(string id)
        {
            Expense item;
            response = await client.GetAsync("api/expense/" + id);
            if (response.IsSuccessStatusCode)
            {
                item = await response.Content.ReadAsAsync<Expense>();
                return item;
            }
            else Console.WriteLine("error while getexpenseasync");
            return null;
        }
        public async Task CreateExpenseAsync(Expense exp)
        {
            //add a new item
            string json = JsonConvert.SerializeObject(exp);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await client.PostAsync("api/expense", content);
            //Console.WriteLine($"status from POST {response.StatusCode}");
            response.EnsureSuccessStatusCode();
            //Console.WriteLine($"added resource at {response.Headers.Location}");
            json = await response.Content.ReadAsStringAsync();
            Console.WriteLine("The chapter has been inserted " + json);
        }
        public async Task<bool> DeleteExpenseAsync(string id)
        {
            response = await client.DeleteAsync("api/expense/" + id);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<Expense> UpdateExpenseAsync(string id,ExpenseToUpdateDto exp2upt)
        {
            string json = JsonConvert.SerializeObject(exp2upt);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await client.PutAsync("api/expense/" + id, content);
            if (response.IsSuccessStatusCode)
            {
                Expense fexp = await response.Content.ReadAsAsync<Expense>();
                return fexp;
            }
            Console.WriteLine("error while updateexpenseasync");
            return null;

        }

    }
}
