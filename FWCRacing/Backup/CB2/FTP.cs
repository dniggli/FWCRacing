using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace CodeBase2
{
    
    public class Ftp
    {
 
        NetworkCredential credentials;

        string FtpURI;
        Uri serverUri;
        public Ftp(string ftpUri)
        {
            serverUri = new Uri(FtpURI);
            if ((serverUri.Scheme != Uri.UriSchemeFtp))
            {
                throw new Exception("Bad URI given");
            }

            FtpURI = ftpUri;
            credentials = new NetworkCredential();
        }

        public Ftp(string FtpUri,string UserName,string Password)
        {
            FtpURI = FtpUri;
            serverUri = new Uri(FtpURI);
            if ((serverUri.Scheme != Uri.UriSchemeFtp))
            {
                throw new Exception("Bad URI given");
            }
            credentials = new NetworkCredential(UserName,Password);
        }

        public string userid
        {
            get { return credentials.UserName; }
            set { credentials.UserName = value; }
        }
        public string password
        {
            get { return credentials.Password; }
            set { credentials.Password = value; }
        }

        /// <summary>
        /// Begins an upload or download request using given filename
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public FtpWebRequest BeginFtpRequest(string filename)
        {
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(FtpURI + filename));
            reqFTP.Credentials = credentials;
            return reqFTP;
        }

        /// <summary>
        /// Begins FTP requests (other than FileUpload)
        /// </summary>
        /// <returns></returns>
        public FtpWebRequest BeginFtpRequest()
        {
            return BeginFtpRequest("");
        }

        public FtpWebResponse BeginFtpQuery(string method)
        {
            FtpWebRequest reqFTP = BeginFtpRequest();

            reqFTP.Method = method;
            reqFTP.Proxy = null;
            return reqFTP.GetResponse() as FtpWebResponse;
        }

        public string FtpQuery(string method)
        {
            FtpWebResponse response;
            string Datastring = "";
            StreamReader sr;
            //try
            //{
            response = BeginFtpQuery(method);
            sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.ASCII);
            Datastring = sr.ReadToEnd();
            // }
            //  catch (Exception ex)
            //  {
            //  Console.WriteLine(ex.Message);
            //    } 
            //	finally
            //	{
            response.Close();
            sr.Close();
            //	}

            return Datastring;
        }

        public string PresentWorkingDirectory()
        {
            return FtpQuery((string)WebRequestMethods.Ftp.PrintWorkingDirectory);
        }

        public string List()
        {
            return FtpQuery((string)WebRequestMethods.Ftp.ListDirectoryDetails);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SavetoPath">localpath c:\\myfolder\\</param>
        /// <param name="SavetoName">filename.txt</param>
        /// <param name="FtpServerName">localhost</param>
        /// <param name="FtpURL">/downloadfolder/downloadthis.txt</param>
        public void Download(string SavetoPath, string SavetoName, string FtpServerName)
        {
            Download(SavetoPath, SavetoName, "ftp://" + FtpServerName + FtpURI);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savetoPath">C:\\Documents and Settings\\cvanvranken\\My Documents</param>
        /// <param name="savetoName">file.txt</param>
        /// <param name="ftpURL">ftp://ServerName/pathto/fileName</param>
        public void Download(string SavetoPath, string SavetoName)
        {
            FtpWebRequest reqFTP = BeginFtpRequest();
            FileStream outputStream = new FileStream(SavetoPath + SavetoName, FileMode.Create);
            //filePath = <<The full path where the file is to be created. the>>, 
            //fileName = <<Name of the file to be createdNeed not name on FTP server. name name()>>

            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            reqFTP.UseBinary = true;
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();


            Stream ftpStream = response.GetResponseStream();
            try
            {



                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    //Write file
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }

        }
        /*
             public bool Upload(string localpath)
                {
                     bool success = false;
				
                        FtpWebRequest ftpReq = BeginFtpRequest();
                        ftpReq.Method = WebRequestMethods.Ftp.UploadFile;
                        Stream s = null;
                        StreamReader stream = new StreamReader(localpath);
                    //	try {            
                            Byte[] b = System.Text.Encoding.UTF8.GetBytes(stream.ReadToEnd());
                                   ftpReq.ContentLength = b.Length;
				
                            s = ftpReq.GetRequestStream();
                        //	try {
                                s.Write(b, 0, b.Length);
                                FtpWebResponse ftpResp= (FtpWebResponse)ftpReq.GetResponse();
                                success=true;
                        //	} catch (Exception ex) {
                        //		Console.WriteLine(ex.Message);
                        //	} finally{	s.Close(); }
					
                    //	} catch (Exception ex) {
                        //	Console.WriteLine(ex.Message);
				
                    //	} finally { 
                        if (s!=null) s.Close();
                        stream.Close(); //}
                

                        return success;
                       // MessageBox.Show(ftpResp.StatusDescription);
                    }
                  
               */

        /// <summary>
        /// Retrives file from server as a string given the location on the server
        /// <para>Throws a WebException if FileNotFound</para>
        /// </summary>
        /// <param name="serverUri">EX: ftp://shlab1/080930_0001.nsf</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DownloadTextFile()
        {
            
            // Get the object used to communicate with the server.
            WebClient request = new WebClient();

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = credentials;

                byte[] newFileData = request.DownloadData(serverUri.ToString());
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);

                return (fileString);

           
    
        }

        /// <summary>
        /// Retrives file from server as a string given the location on the server
        /// <para>Throws a WebException if FileNotFound</para>
        /// </summary>
        /// <param name="serverUri">EX: ftp://shlab1/080930_0001.nsf</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public void UploadTextFile(string localpath,string filename)
        {

            // Get the object used to communicate with the server.
            WebClient request = new WebClient();

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = credentials;

            StreamReader stream = new StreamReader(localpath + "\\" + filename);
                Byte[] FileData = System.Text.Encoding.UTF8.GetBytes(stream.ReadToEnd());
                stream.Close();
            

            request.UploadData(serverUri, FileData);     
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="localpath">c:\users</param>
        /// <param name="filename">test.txt</param>
        /// <returns></returns>
        public bool Upload(string localpath,string filename)
        {
            bool success = false;

           
                FtpWebRequest ftpReq = BeginFtpRequest(filename);

            
                //  FtpWebRequest ftpReq = (FtpWebRequest)FtpWebRequest.Create(FtpURL);
                ftpReq.Method = WebRequestMethods.Ftp.UploadFile;
                //ftpReq.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                StreamReader stream = new StreamReader(localpath + "\\" + filename);
                Byte[] b = System.Text.Encoding.UTF8.GetBytes(stream.ReadToEnd());
                stream.Close();

                ftpReq.ContentLength = b.Length;
                ftpReq.UsePassive = true;
                ftpReq.KeepAlive = true; ;
                ftpReq.UseBinary = true;
               
                Stream s = ftpReq.GetRequestStream();
                s.Write(b, 0, b.Length);
                s.Close();
            
                FtpWebResponse ftpResp = (FtpWebResponse)ftpReq.GetResponse();
                Console.WriteLine("(" + ftpResp.StatusCode.ToString() + ")" + ftpResp.StatusDescription);

                success = true;

            return success;
        }
    }
}
