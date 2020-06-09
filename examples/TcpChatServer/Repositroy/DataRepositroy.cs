using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using TcpChatServer.Model;

namespace TcpChatServer.Repositroy
{
   public class DataRepositroy
    {
        public async Task InsertData(string connectionString,DataPacket dataPacket)
        {
            // define INSERT query with parameters
            var query = "INSERT INTO DeviceReadings (Date, Adc, Course, Crc16, CreatedAt, Hdop,Height,IButton,Input,Lan,Lan1,Lat,Lat1,Output,PacketType,Params,Speed,Stats) " +
                                            "VALUES (@Date,@Adc, @Course, @Crc16, @CreatedAt, @Hdop, @Height,@IButton,@Input,@Lan,@Lan1,@Lat,@Lat1,@Output,@PacketType,@Params,@Speed,@Stats) ";
            // create connection and command
            using(var cn = new SqlConnection(connectionString))
            using(var cmd = new SqlCommand(query, cn))
            {
                // define parameters and their values
                cmd.Parameters.Add("@Date", SqlDbType.VarChar, 50).Value = dataPacket.Date;
                cmd.Parameters.Add("@Adc", SqlDbType.VarChar, 50).Value = dataPacket.Adc;
                cmd.Parameters.Add("@Course", SqlDbType.VarChar, 50).Value = dataPacket.Course;
                cmd.Parameters.Add("@Crc16", SqlDbType.VarChar, 50).Value = dataPacket.Crc16;
                cmd.Parameters.Add("@CreatedAt", SqlDbType.VarChar, 50).Value = dataPacket.CreatedAt;
                cmd.Parameters.Add("@Hdop", SqlDbType.VarChar, 50).Value = dataPacket.Hdop;
                cmd.Parameters.Add("@Height", SqlDbType.VarChar, 50).Value = dataPacket.Height;
                cmd.Parameters.Add("@IButton", SqlDbType.VarChar, 50).Value = dataPacket.IButton;
                cmd.Parameters.Add("@Input", SqlDbType.VarChar, 50).Value = dataPacket.Input;
                cmd.Parameters.Add("@Lan", SqlDbType.VarChar, 50).Value = dataPacket.Lan;
                cmd.Parameters.Add("@Lan1", SqlDbType.VarChar, 50).Value = dataPacket.Lan1;
                cmd.Parameters.Add("@Lat", SqlDbType.VarChar, 50).Value = dataPacket.Lat;
                cmd.Parameters.Add("@Lat1", SqlDbType.VarChar, 50).Value = dataPacket.Lat1;
                cmd.Parameters.Add("@Output", SqlDbType.VarChar, 50).Value = dataPacket.Output;
                cmd.Parameters.Add("@PacketType", SqlDbType.VarChar, 50).Value = dataPacket.PacketType;
                cmd.Parameters.Add("@Params", SqlDbType.VarChar, 50).Value = dataPacket.Params;
                cmd.Parameters.Add("@Speed", SqlDbType.VarChar, 50).Value = dataPacket.Speed;
                cmd.Parameters.Add("@Stats", SqlDbType.VarChar, 50).Value = dataPacket.Stats;

                // open connection, execute INSERT, close connection
                cn.Open();
                cmd.ExecuteNonQueryAsync();
                cn.Close();
            }
        }

        public static int IsertRaw(string connectionString,DeviceRawData deviceRawData)
        {
            var query = "INSERT INTO DeviceRawDatas (DeviceLogin, DeviceData, IsSync, CreationTime,IsFailedToMatch,IsDeleted) " +
                                           "VALUES (@DeviceLogin,@DeviceData,@IsSync,@CreationTime,@IsFailedToMatch,@IsDeleted) ";
            // create connection and command
            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, cn))
            {
                // define parameters and their values
                cmd.Parameters.Add("@DeviceLogin", SqlDbType.VarChar, 100).Value = deviceRawData.DeviceLogin;
                cmd.Parameters.Add("@DeviceData", SqlDbType.VarChar, 1000).Value = deviceRawData.RawData;
                cmd.Parameters.Add("@IsSync", SqlDbType.Bit).Value = deviceRawData.IsSync;
                cmd.Parameters.Add("@CreationTime", SqlDbType.DateTime2).Value = deviceRawData.CreateAt;
                cmd.Parameters.Add("@IsFailedToMatch", SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit).Value = false;
                
                // open connection, execute INSERT, close connection
                cn.Open();
               var response= cmd.ExecuteNonQuery();
                cn.Close();
                return response;
            }
        }
    }
}
