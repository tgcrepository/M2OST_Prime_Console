// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.SocialAuthModel
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using MySql.Data.MySqlClient;
using System;
using System.Configuration;

namespace IBHFL.Models
{
  public class SocialAuthModel
  {
    private MySqlConnection connection;

    public SocialAuthModel() => this.connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);

    public SocailUserDetails GetSocialUserID(string request)
    {
      SocailUserDetails socialUserId = new SocailUserDetails();
      try
      {
        string str = "SELECT ID_USER,USERID,GPSOCIALID,ID_ORGANIZATION,ID_CODE FROM tbl_user WHERE GPSOCIALID = @value1 AND status = 'A'";
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("value1", (object) request);
        MySqlDataReader mySqlDataReader = command.ExecuteReader();
        while (mySqlDataReader.Read())
          socialUserId = new SocailUserDetails()
          {
            social_name = mySqlDataReader["USERID"].ToString(),
            social_id = mySqlDataReader["GPSOCIALID"].ToString(),
            id_organisation = Convert.ToInt32(mySqlDataReader["ID_ORGANIZATION"]),
            ID_USER = Convert.ToInt32(mySqlDataReader["ID_USER"]),
            ID_Code = Convert.ToInt32(mySqlDataReader["ID_CODE"])
          };
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
      return socialUserId;
    }

    public int InsertSocialUser(SocailUserDetails request)
    {
      try
      {
        string str = "INSERT INTO tbl_user ( ID_CODE, ID_ORGANIZATION, ID_ROLE, USERID,PASSWORD,FBSOCIALID, GPSOCIALID, STATUS, UPDATEDTIME,user_status)" + "VALUES (@ID_CODE, @ID_ORGANIZATION, @ID_ROLE, @USERID, @PASSWORD,@FBSOCIALID, @GPSOCIALID, @STATUS, @UPDATEDTIME, @user_status)";
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("ID_CODE", (object) 1);
        command.Parameters.AddWithValue("ID_ORGANIZATION", (object) 16);
        command.Parameters.AddWithValue("ID_ROLE", (object) 11);
        command.Parameters.AddWithValue("PASSWORD", (object) request.spasswd);
        command.Parameters.AddWithValue("USERID", (object) request.social_email);
        command.Parameters.AddWithValue("FBSOCIALID", (object) "");
        command.Parameters.AddWithValue("GPSOCIALID", (object) request.social_id);
        command.Parameters.AddWithValue("STATUS", (object) 'A');
        command.Parameters.AddWithValue("UPDATEDTIME", (object) DateTime.Now);
        command.Parameters.AddWithValue("user_status", (object) 'A');
        int num = command.ExecuteNonQuery();
        return num == 1 ? Convert.ToInt32(command.LastInsertedId) : num;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
    }

    public int InsertSocialUserProfile(SocailUserDetails request)
    {
      try
      {
        string str = "INSERT INTO tbl_profile(ID_USER,FIRSTNAME,LASTNAME,AGE,LOCATION,EMAIL,MOBILE)" + "VALUES(@ID_USER,@FIRSTNAME,@LASTNAME,@AGE,@LOCATION,@EMAIL,@MOBILE)";
        this.connection.Open();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = str;
        command.Parameters.AddWithValue("ID_USER", (object) request.ID_USER);
        command.Parameters.AddWithValue("FIRSTNAME", (object) request.social_name);
        command.Parameters.AddWithValue("LASTNAME", (object) request.lastname);
        command.Parameters.AddWithValue("AGE", (object) 0);
        command.Parameters.AddWithValue("LOCATION", (object) "-");
        command.Parameters.AddWithValue("EMAIL", (object) request.social_email);
        command.Parameters.AddWithValue("MOBILE", (object) "");
        int num = command.ExecuteNonQuery();
        return num == 1 ? Convert.ToInt32(command.LastInsertedId) : num;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.connection.Close();
      }
    }
  }
}
