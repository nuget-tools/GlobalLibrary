using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;

namespace Global;
public class WebGet
{
    public WebClient Client = new WebClient();
    public int StatusCode;
    public string StatusName;

    public WebHeaderCollection Headers
    {
        get
        {
            return Client.Headers;
        }
        set
        {
            Client.Headers = value;
        }
    }
    internal WebHeaderCollection _responseHeaders;
    public WebHeaderCollection ResponseHeaders
    {
        get
        {
            return _responseHeaders; // Client.ResponseHeaders;
        }

    }
    public event EventHandler<ProgressEventArgs> OnProgress;

    public class ProgressEventArgs : EventArgs
    {
        public bool Complete { get; set; }
        public long Progress { get; set; }
        public long? Length { get; set; }
        public double Percent { get; set; }
    }

    protected void OnProgressEvent(bool complete, long progress, long? length, double percent)
    {
        OnProgress?.Invoke(this, new ProgressEventArgs
        {
            Complete = complete,
            Progress = progress,
            Length = length,
            Percent = percent
        });
    }


    public byte[] Get(string url, NameValueCollection query = null)
    {
        if (query != null)
        {
            string q = String.Join("&", query.AllKeys.Select(a => a + "=" + System.Web.HttpUtility.UrlEncode(query[a])));
            if (url.Contains("?"))
            {
                url = url + "&" + q;
            }
            else
            {
                url = url + "?" + q;
            }

        }
        try
        {
            using (Stream stream = Client.OpenRead(url))
            {
                this.StatusCode = 200;
                this.StatusName = "OK";
                WebHeaderCollection headers = Client.ResponseHeaders;
                this._responseHeaders = headers;
                string contentLengthHeader = Client.ResponseHeaders["Content-Length"];
                //Util.Print(contentLengthHeader, "contentLength");
                long? contentLength = null;
                long n;
                if (long.TryParse(contentLengthHeader, out n))
                {
                    contentLength = n;
                }
                else
                {
                    contentLength = null;
                }
                byte[] buffer = new byte[1024];
                int bytesRead;
                MemoryStream ms = new MemoryStream();
                long total = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, bytesRead);
                    total += bytesRead;
                    if (contentLength == null)
                        OnProgressEvent(false, total, contentLength, 0.0);
                    else
                        OnProgressEvent(false, total, contentLength, (total * 100.0 / contentLength).GetValueOrDefault(0.0));
                }
                OnProgressEvent(true, total, contentLength, 100.0);
                return ms.ToArray();
            }
        }
        catch (WebException we)
        {
            HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;
            this.StatusCode = (int)response.StatusCode;
            this.StatusName = response.StatusCode.ToString();
            this._responseHeaders = response.Headers;
            using (Stream responseStream = response.GetResponseStream())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    responseStream.CopyTo(ms);
                    byte[] responseBody = ms.ToArray();
                    return responseBody;
                }
            }
        }
    }
}
