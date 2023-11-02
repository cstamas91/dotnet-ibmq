namespace CsT.Dotnet.IbmQ.Core;

class ConnectionPropertyBuilder
{
    private const string CCDT = "MQCCDTURL";
    private const string FILEPREFIX = "file://";
    static public void SetConnectionProperties(IConnectionFactory cf, Env env)
    {
        Env.ConnVariables? conn = env.Conn;

        string ccdtURL = CheckForCCDT();
        if (null != ccdtURL)
        {
            Console.WriteLine("CCDT Environment setting found");
            cf.SetStringProperty(XMSC.WMQ_CCDTURL, ccdtURL);
        }
        else
        {
            if (env.NumberOfConnections() > 1)
            {
                Console.WriteLine("There are {0} connections", env.NumberOfConnections());
                cf.SetStringProperty(XMSC.WMQ_CONNECTION_NAME_LIST, env.BuildConnectionString());
                Console.WriteLine("Connection string is {0}", env.BuildConnectionString());
            }
            else
            {
                cf.SetStringProperty(XMSC.WMQ_HOST_NAME, conn?.host);
                Console.WriteLine("hostName is set {0, -20 }", conn?.host);
                cf.SetIntProperty(XMSC.WMQ_PORT, conn?.port ?? 0);
            }
            cf.SetStringProperty(XMSC.WMQ_CHANNEL, conn?.channel);
        }
        SetRemConnectionProperties(cf, conn);
    }

    static public void SetConnectionProperties(IConnectionFactory cf, Env.ConnVariables conn)
    {
        string ccdtURL = CheckForCCDT();
        if (null != ccdtURL)
        {
            Console.WriteLine("CCDT Environment setting found");
            cf.SetStringProperty(XMSC.WMQ_CCDTURL, ccdtURL);
        }
        else
        {
            cf.SetStringProperty(XMSC.WMQ_HOST_NAME, conn.host);
            Console.WriteLine("hostName is set {0, -20 }", conn.host);
            cf.SetIntProperty(XMSC.WMQ_PORT, conn.port);
            cf.SetStringProperty(XMSC.WMQ_CHANNEL, conn.channel);
        }
        SetRemConnectionProperties(cf, conn);
    }

    static private void SetRemConnectionProperties(IConnectionFactory cf, Env.ConnVariables? conn)
    {
        cf.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, conn?.is_managed ?? false ? XMSC.WMQ_CM_CLIENT : XMSC.WMQ_CM_CLIENT_UNMANAGED);
        cf.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, conn?.qmgr);
        cf.SetStringProperty(XMSC.USERID, conn?.app_user);
        cf.SetStringProperty(XMSC.PASSWORD, conn?.app_password);

        Console.WriteLine("Connection Cipher is set to {0}", conn?.cipher_suite);
        Console.WriteLine("Key Repository is set to {0}", conn?.key_repository);

        if (conn?.key_repository != null && conn.cipher_suite != null)
        {
            cf.SetStringProperty(XMSC.WMQ_SSL_KEY_REPOSITORY, conn.key_repository);
        }
        if (conn?.cipher_suite != null)
        {
            cf.SetStringProperty(XMSC.WMQ_SSL_CIPHER_SPEC, conn.cipher_suite);
        }
    }

    static private string CheckForCCDT()
    {
        Console.WriteLine("Checking for CCDT File");
        string ccdt = Environment.GetEnvironmentVariable(CCDT) ?? string.Empty;

        if (string.IsNullOrEmpty(ccdt))
        {
            Console.WriteLine("{0} environment variable is set to {1}", CCDT, ccdt);
            Console.WriteLine("Will be checking for {0}", ccdt.Replace(FILEPREFIX, ""));
            if (File.Exists(ccdt.Replace(FILEPREFIX, "")))
            {
                Console.WriteLine("CCDT file found");
                return ccdt;
            }
        }

        Console.WriteLine("No CCDT file found or specified");
        return string.Empty;
    }

}
