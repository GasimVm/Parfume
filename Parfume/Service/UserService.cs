using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Service
{
    public class UserService : IUserService
    {
        public (IEnumerable<CustomerModel> customers, int rowCount) GetCustomer(string fincode, int page, int length, string search )
        {
            List<CustomerModel> employees = new List<CustomerModel>();
            int rowCount = 0;
            search = search?.ToLower().Replace(" ", "");
            using (SqlConnection con = new SqlConnection(AppConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SP_GET_CUSTOMER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@length", length);
                    cmd.Parameters.AddWithValue("@search", (object)search ?? DBNull.Value);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //employees.Add(new CustomerModel
                            //{
                            //    Id = (int)reader["Id"],
                            //    Fincode = reader["Fincode"]?.ToString(),
                            //    Name = reader["Name"]?.ToString(),
                            //    Surname = reader["Surname"]?.ToString(),
                            //    FatherName = reader["FatherName"]?.ToString(),
                            //    BaseNumber = reader["BaseNumber"]?.ToString(),
                            //    Address = reader["Address"]?.ToString(),
                            //    WorkAddress = reader["WorkAddress"]?.ToString(),
                            //    InstagramAddress = reader["InstagramAddress"]?.ToString(),
                            //    FirstNumber = reader["FirstNumber"]?.ToString(),
                            //    FirstNumberWho = reader["FirstNumberWho"]?.ToString(),
                            //    SecondNumber = reader["SecondNumber"]?.ToString(),
                            //    SecondNumberWho = reader["SecondNumberWho"]?.ToString(),
                            //    ThirdNumber = reader["ThirdNumber"]?.ToString(),
                            //    ThirdNumberWho = reader["ThirdNumberWho"]?.ToString(),
                            //});
                            var test = reader[23]?.ToString();
                            employees.Add(new CustomerModel
                            {
                                Id = (int)reader[0],
                                Fincode = reader[4]?.ToString(),
                                Name = reader[1]?.ToString(),
                                Surname = reader[2]?.ToString(),
                                FatherName = reader[3]?.ToString(),
                                BaseNumber = reader[8]?.ToString(),
                                Address = reader[5]?.ToString(),
                                WorkAddress = reader[6]?.ToString(),
                                InstagramAddress = reader[7]?.ToString(),
                                FirstNumber = reader[9]?.ToString(),
                                FirstNumberWho = reader[10]?.ToString(),
                                SecondNumber = reader[11]?.ToString(),
                                SecondNumberWho = reader[12]?.ToString(),
                                ThirdNumber = reader[13]?.ToString(),
                                ThirdNumberWho = reader[14]?.ToString(),
                                WhoIsOkey = reader[21]?.ToString(),
                                CardId=reader[23]?.ToString()
                            });
                        }

                        if (reader.NextResult()
                            && reader.HasRows
                            && reader.Read())
                        {
                            rowCount = (int)reader["ROW_COUNT"];
                        }
                    }
                }
            }
            return (employees, rowCount);
        }

        public IEnumerable<CustomerModel> GetCustomerWithDebt(int type)
        {
            List<CustomerModel> employees = new List<CustomerModel>();
            using (SqlConnection con = new SqlConnection(AppConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(NameProsedur(type), con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IsBlock", false);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            employees.Add(new CustomerModel
                            {
                                Id = (int)reader[0],
                                Fincode = reader[4]?.ToString(),
                                Name = reader[1]?.ToString(),
                                Surname = reader[2]?.ToString(),
                                FatherName = reader[3]?.ToString(),
                                BaseNumber = reader[8]?.ToString(),
                                Address = reader[5]?.ToString(),
                                WorkAddress = reader[6]?.ToString(),
                                InstagramAddress = reader[7]?.ToString(),
                                FirstNumber = reader[9]?.ToString(),
                                FirstNumberWho = reader[10]?.ToString(),
                                SecondNumber = reader[11]?.ToString(),
                                SecondNumberWho = reader[12]?.ToString(),
                                ThirdNumber = reader[13]?.ToString(),
                                ThirdNumberWho = reader[14]?.ToString(),
                                WhoIsOkey = reader[21]?.ToString(),
                                Note = reader[20]?.ToString(),
                                BlockNote = reader[19]?.ToString(),
                                Birthday= reader[22]?.ToString(),
                                
                            });
                        }
                    }
                }
            }
            return (employees);
        }

        public (IEnumerable<CustomerModel> customers, int rowCount) GetCustomerWithPhone(string fincode, int page, int length, string search)
        {
            List<CustomerModel> employees = new List<CustomerModel>();
            int rowCount = 0;
            search = search?.ToLower().Replace(" ", "");
            using (SqlConnection con = new SqlConnection(AppConfig.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SP_GET_CUSTOMER_WITH_PHONE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@length", length);
                    cmd.Parameters.AddWithValue("@search", (object)search ?? DBNull.Value);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            employees.Add(new CustomerModel
                            {
                                Id = (int)reader[0],
                                Fincode = reader[4]?.ToString(),
                                Name = reader[1]?.ToString(),
                                Surname = reader[2]?.ToString(),
                                FatherName = reader[3]?.ToString(),
                                BaseNumber = reader[8]?.ToString(),
                                Address = reader[5]?.ToString(),
                                WorkAddress = reader[6]?.ToString(),
                                InstagramAddress = reader[7]?.ToString(),
                                FirstNumber = reader[9]?.ToString(),
                                FirstNumberWho = reader[10]?.ToString(),
                                SecondNumber = reader[11]?.ToString(),
                                SecondNumberWho = reader[12]?.ToString(),
                                ThirdNumber = reader[13]?.ToString(),
                                ThirdNumberWho = reader[14]?.ToString(),
                                WhoIsOkey = reader[21]?.ToString(),
                            });
                        }

                        if (reader.NextResult()
                            && reader.HasRows
                            && reader.Read())
                        {
                            rowCount = (int)reader["ROW_COUNT"];
                        }
                    }
                }
            }
            return (employees, rowCount);
        }

        public string NameProsedur(int type)
        {
           string prosedurName = "";
            if (type == 1)
                prosedurName = "[dbo].[SP_GET_CUSTOMER_WITH_DEBT]";
            else if(type == 2)
                prosedurName = "[dbo].[SP_GET_CUSTOMER_WITH_OUT_DEBT]";
            else if (type == 0)
                prosedurName = "[dbo].[SP_GET_CUSTOMER_WITH_OUT_ORDER]";
            return prosedurName;
        }
    }
}
