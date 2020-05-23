using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using TcpChatServer.Model;

namespace TcpChatServer.Heleper
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this string s,
            string format = "ddMMyy", string cultureString = "tr-TR")
        {
            try
            {
                var r = DateTime.ParseExact(
                    s: s,
                    format: format,
                    provider: CultureInfo.GetCultureInfo(cultureString));
                return r;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CultureNotFoundException)
            {
                throw; // Given Culture is not supported culture
            }
        }

        public static DateTime ToDateTime(this string s,
            string format, CultureInfo culture)
        {
            try
            {
                var r = DateTime.ParseExact(s: s, format: format,
                    provider: culture);
                return r;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CultureNotFoundException)
            {
                throw; // Given Culture is not supported culture
            }

        }

    }

    public class ParserHeleper
    {
        private void fileWrite(string file)
        {

            string path = @"C:\Users\Administrator\Desktop\Newfolder\data2.txt";
            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(file);

                }
            }

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(file);
            }
        }
        private static string AddChunkSeparator(string str, int chunk_len, char separator)
        {
            if (str == null || str.Length < chunk_len)
            {
                return str;
            }

            int count = 0;
            StringBuilder builder = new StringBuilder();
            for (var index = 0; index < str.Length; index += chunk_len)
            {
                builder.Append(str, index, chunk_len);
                if (count < 2)
                {
                    builder.Append(separator);
                }
                count++;
            }

            count = 0;
            return builder.ToString();
        }
        private static DataPacket DataParse(string data)
        {
            var response = data.Split(";");
            if (response.Length > 5)
            {
                var dataPacket = new DataPacket();
                dataPacket.CreatedAt = DateTime.Now;
                dataPacket.PacketType = PacketType.D;
                dataPacket.ThreadId = "1";
                dataPacket.Date = response[0].Trim().ToDateTime(format: "ddMMyy");
                dataPacket.Date =
                    dataPacket.Date.Date.Add(DateTime.ParseExact(AddChunkSeparator(response[1].Trim(), 2, ':'), "HH:mm:ss", CultureInfo.InvariantCulture)
                        .TimeOfDay);
                dataPacket.Lat = response[2];
                dataPacket.Lat1 = response[3];
                dataPacket.Lan = response[4];
                dataPacket.Lan1 = response[5];
                dataPacket.Speed = response[6];
                dataPacket.Course = response[7];
                dataPacket.Height = response[8];
                dataPacket.Stats = response[9];
                dataPacket.Hdop = response[10];
                dataPacket.Input = response[11];
                dataPacket.Output = response[12];
                dataPacket.Adc = response[13];
                dataPacket.IButton = response[14];
                dataPacket.Params = response[15];
                return dataPacket;
            }
            return null;
        }
      
        public void DataPacketParser(string data)
        {
            fileWrite(data);

            // var dataPacket = new DataPacket();
            //type
            //data = "#B#281119;073044;5351.7216;N;02741.3750;E;0.000;232.336;266.875;6;2.730;NA;NA;NA;NA;100_521347:1:521246,100_521129:1:1,100_521050:1:1,100_521106:3:Technoton|281119;073043;5351.7216;N;02741.3750;E;0.000;62.664;265.750;6;2.730;NA;NA;NA;NA;100_521347:1:521246"
            //    + ",100_158:1:300,100_521055:1:300,100_521056:1:4,100_521488:1:4194304,100_521050:1:1,100_521129:1:1,100_521072_21.2:1:130,100_521106:3:Technoton | 281119; 072947; 5351.7216; N; 02741.3750; E; 0.000; 253.008; 270.125; 6; 2.740; NA; NA; NA; NA; 100_521347:1:521246 | 281119; 07294"
            //    + "6; 5351.7216; N; 02741.3750; E; 0.000; 253.008; 270.125; 6; 2.740; NA; NA; NA; NA; 100_521347:1:521246 | 281119; 072945; 5351.7216; N; 02741.3750; E; 0.000; 253.008; 270.125; 6; 2.740; NA; NA; NA; NA; 100_521347:1:521246,100_521106:3:Technoton | 281119; 072944; 5351.7216; N; 02741.3750; E; 0.00"
            //    + "0; 246.789; 270.375; 6; 2.740; NA; NA; NA; NA; 100_521347:1:521246,100_521129:1:1,100_521050:1:1,100_521106:3:Technoton | F48";

            //  data = "#D#271119;212102;5351.6080;N;02741.1311;E; 0.000;61.266;131.125;9;1.500;NA;NA;NA;NA;101_521347:1:521246,101_158:1:298,101_521055:1:298,101_521055_2.9:1:297,101_521056:1:1;DED4";

            //if (data.Contains("#D#"))
            //{
            //    var response = data.Split("#")[2];
            //    dataPacket.Crc16 = response.Split(";")[16];
            //}
            //else if (data.Contains("#B#"))
            //{
            //    var response = data.Split("#")[2].Split("|");
            //    foreach (var item in response)
            //    {
            //        var response2 = DataParse(item);
            //    }
            //}
            //else
            //{

            //}

            //length
        }
    }
}
