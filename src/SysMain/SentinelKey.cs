/***************************************************************************
* SentinelKey.cs
*
* (C) Copyright 2009 SafeNet, Inc. All rights reserved.
*
* Description - This module provide defination and wrappers for 
*               Sentinel Keys Client Library APIs
*
****************************************************************************/
using System;
public class SentinelKey : IDisposable
{
	#region Constructor / Destructor
	
	private uint licHandle;
	private bool disposed = false;

	private string strError = "Unable to load the required SentinelKey Library(SentinelKeyW.dll).\n"+
							"Either the library is missing or corrupted.";

	public SentinelKey()
	{
		
	}

	~SentinelKey()
	{
		Dispose(false);
	}

	/* Implement IDisposable */
	public void Dispose()
	{
		Dispose(true);
	}
	/* Private class specifically for this object.*/
	public void Dispose(bool disposing)	
	{
		if(!this.disposed)
		{
			if(disposing)				
			{
				this.licHandle = 0;
				/* only MANAGED resource should be dispose     */
				/* here that implement the IDisposable		   */
				/* Interface 								   */
			}
			/* only UNMANAGED resource should be dispose here */
			/*												  */
		}
		disposed = true;         
	}
	#endregion

	#region SentinelKey API Error Codes
	/* SentinelKey API Error codes */ 
	
	public const int SP_DRIVER_LIBRARY_ERROR_BASE		= 100;
	public const int SP_DUAL_LIBRARY_ERROR_BASE			= 200;
	public const int  SP_SERVER_ERROR_BASE				= 300;
	public const int  SP_SHELL_ERROR_BASE				= 400;
	public const int  SP_SECURE_UPDATE_ERROR_BASE		= 500;
    public const int  SP_SETUP_LIBRARY_ERROR_BASE       = 700;
	public const int  SP_SUCCESS = 0;

	//Dual Library Error Codes:
	public const int  SP_ERR_INVALID_PARAMETER			= (SP_DUAL_LIBRARY_ERROR_BASE + 1);
	public const int  SP_ERR_SOFTWARE_KEY				= (SP_DUAL_LIBRARY_ERROR_BASE + 2);
	public const int  SP_ERR_INVALID_LICENSE			= (SP_DUAL_LIBRARY_ERROR_BASE + 3);
	public const int  SP_ERR_INVALID_FEATURE			= (SP_DUAL_LIBRARY_ERROR_BASE + 4);
	public const int  SP_ERR_INVALID_TOKEN				= (SP_DUAL_LIBRARY_ERROR_BASE + 5);
	public const int  SP_ERR_NO_LICENSE					= (SP_DUAL_LIBRARY_ERROR_BASE + 6);
	public const int  SP_ERR_INSUFFICIENT_BUFFER		= (SP_DUAL_LIBRARY_ERROR_BASE + 7);
	public const int  SP_ERR_VERIFY_FAILED				= (SP_DUAL_LIBRARY_ERROR_BASE + 8);
	public const int  SP_ERR_CANNOT_OPEN_DRIVER			= (SP_DUAL_LIBRARY_ERROR_BASE + 9);
	public const int  SP_ERR_ACCESS_DENIED				= (SP_DUAL_LIBRARY_ERROR_BASE + 10);
	public const int  SP_ERR_INVALID_DEVICE_RESPONSE	= (SP_DUAL_LIBRARY_ERROR_BASE + 11);
	public const int  SP_ERR_COMMUNICATIONS_ERROR		= (SP_DUAL_LIBRARY_ERROR_BASE + 12);
	public const int  SP_ERR_COUNTER_LIMIT				= (SP_DUAL_LIBRARY_ERROR_BASE + 13);
	public const int  SP_ERR_MEM_CORRUPT				= (SP_DUAL_LIBRARY_ERROR_BASE + 14);
	public const int  SP_ERR_INVALID_FEATURE_TYPE		= (SP_DUAL_LIBRARY_ERROR_BASE + 15);
	public const int  SP_ERR_DEVICE_IN_USE				= (SP_DUAL_LIBRARY_ERROR_BASE + 16);
	public const int  SP_ERR_INVALID_API_VERSION		= (SP_DUAL_LIBRARY_ERROR_BASE + 17);
	public const int  SP_ERR_TIME_OUT_ERROR				= (SP_DUAL_LIBRARY_ERROR_BASE + 18);
	public const int  SP_ERR_INVALID_PACKET				= (SP_DUAL_LIBRARY_ERROR_BASE + 19);
	public const int  SP_ERR_KEY_NOT_ACTIVE				= (SP_DUAL_LIBRARY_ERROR_BASE + 20);
	public const int  SP_ERR_FUNCTION_NOT_ENABLED		= (SP_DUAL_LIBRARY_ERROR_BASE + 21);
	public const int  SP_ERR_DEVICE_RESET				= (SP_DUAL_LIBRARY_ERROR_BASE + 22);
	public const int  SP_ERR_TIME_CHEAT					= (SP_DUAL_LIBRARY_ERROR_BASE + 23);
	public const int  SP_ERR_INVALID_COMMAND			= (SP_DUAL_LIBRARY_ERROR_BASE + 24);
	public const int  SP_ERR_RESOURCE					= (SP_DUAL_LIBRARY_ERROR_BASE + 25);
	public const int  SP_ERR_UNIT_NOT_FOUND				= (SP_DUAL_LIBRARY_ERROR_BASE + 26);
	public const int  SP_ERR_DEMO_EXPIRED				= (SP_DUAL_LIBRARY_ERROR_BASE + 27);
	public const int  SP_ERR_QUERY_TOO_LONG				= (SP_DUAL_LIBRARY_ERROR_BASE + 28);
	public const int SP_ERR_USER_AUTH_REQUIRED = (SP_DUAL_LIBRARY_ERROR_BASE + 29);
    public const int SP_ERR_DUPLICATE_LIC_ID        = (SP_DUAL_LIBRARY_ERROR_BASE + 30);
    public const int SP_ERR_DECRYPTION_FAILED       = (SP_DUAL_LIBRARY_ERROR_BASE + 31);
    public const int SP_ERR_BAD_CHKSUM              = (SP_DUAL_LIBRARY_ERROR_BASE + 32);
    public const int SP_ERR_BAD_LICENSE_IMAGE       = (SP_DUAL_LIBRARY_ERROR_BASE + 33);
    public const int SP_ERR_INSUFFICIENT_MEMORY     = (SP_DUAL_LIBRARY_ERROR_BASE + 34);
    public const int SP_ERR_CONFIG_FILE_NOT_FOUND     = (SP_DUAL_LIBRARY_ERROR_BASE + 35);


	//Server Error Codes
	public const int  SP_ERR_SERVER_PROBABLY_NOT_UP		= (SP_SERVER_ERROR_BASE + 1);
	public const int  SP_ERR_UNKNOWN_HOST				= (SP_SERVER_ERROR_BASE + 2);
	public const int  SP_ERR_BAD_SERVER_MESSAGE			= (SP_SERVER_ERROR_BASE + 3);
	public const int  SP_ERR_NO_LICENSE_AVAILABLE		= (SP_SERVER_ERROR_BASE + 4);
	public const int  SP_ERR_INVALID_OPERATION			= (SP_SERVER_ERROR_BASE + 5);
	public const int  SP_ERR_INTERNAL_ERROR				= (SP_SERVER_ERROR_BASE + 6);
	public const int  SP_ERR_PROTOCOL_NOT_INSTALLED		= (SP_SERVER_ERROR_BASE + 7);
	public const int  SP_ERR_BAD_CLIENT_MESSAGE			= (SP_SERVER_ERROR_BASE + 8);
	public const int  SP_ERR_SOCKET_OPERATION			= (SP_SERVER_ERROR_BASE + 9);
	public const int  SP_ERR_NO_SERVER_RESPONSE         = (SP_SERVER_ERROR_BASE + 10);
	public const int SP_ERR_SERVER_BUSY                 = (SP_SERVER_ERROR_BASE + 11);
	public const int SP_ERR_SERVER_TIME_OUT             = (SP_SERVER_ERROR_BASE + 12);

	// Shell Error Codes 
	public const int  SP_ERR_BAD_ALGO					= (SP_SHELL_ERROR_BASE + 1);
	public const int  SP_ERR_LONG_MSG					= (SP_SHELL_ERROR_BASE + 2);
	public const int  SP_ERR_READ_ERROR					= (SP_SHELL_ERROR_BASE + 3);
	public const int  SP_ERR_NOT_ENOUGH_MEMORY			= (SP_SHELL_ERROR_BASE + 4);
	public const int  SP_ERR_CANNOT_OPEN				= (SP_SHELL_ERROR_BASE + 5);
	public const int  SP_ERR_WRITE_ERROR				= (SP_SHELL_ERROR_BASE + 6);
	public const int  SP_ERR_CANNOT_OVERWRITE			= (SP_SHELL_ERROR_BASE + 7);
	public const int  SP_ERR_INVALID_HEADER				= (SP_SHELL_ERROR_BASE + 8);
	public const int  SP_ERR_TMP_CREATE_ERROR			= (SP_SHELL_ERROR_BASE + 9);
	public const int  SP_ERR_PATH_NOT_THERE				= (SP_SHELL_ERROR_BASE + 10);
	public const int  SP_ERR_BAD_FILE_INFO				= (SP_SHELL_ERROR_BASE + 11);
	public const int  SP_ERR_NOT_WIN32_FILE				= (SP_SHELL_ERROR_BASE + 12);
	public const int  SP_ERR_INVALID_MACHINE			= (SP_SHELL_ERROR_BASE + 13);
	public const int  SP_ERR_INVALID_SECTION			= (SP_SHELL_ERROR_BASE + 14);
	public const int  SP_ERR_INVALID_RELOC				= (SP_SHELL_ERROR_BASE + 15);
	public const int  SP_ERR_CRYPT_ERROR				= (SP_SHELL_ERROR_BASE + 16);
	public const int  SP_ERR_SMARTHEAP_ERROR			= (SP_SHELL_ERROR_BASE + 17);
	public const int  SP_ERR_IMPORT_OVERWRITE_ERROR		= (SP_SHELL_ERROR_BASE + 18);
    public const int  SP_ERR_FRAMEWORK_REQUIRED         = (SP_SHELL_ERROR_BASE + 21);
    public const int SP_ERR_CANNOT_HANDLE_FILE          = (SP_SHELL_ERROR_BASE + 22);
	
	public const int  SP_ERR_STRONG_NAME = (SP_SHELL_ERROR_BASE + 26);
    public const int  SP_ERR_FRAMEWORK_10 = (SP_SHELL_ERROR_BASE + 27);
    public const int  SP_ERR_FRAMEWORK_SDK_10 = (SP_SHELL_ERROR_BASE + 28);
    public const int  SP_ERR_FRAMEWORK_11 = (SP_SHELL_ERROR_BASE + 29);
    public const int  SP_ERR_FRAMEWORK_SDK_11 = (SP_SHELL_ERROR_BASE + 30);
    public const int  SP_ERR_FRAMEWORK_20 = (SP_SHELL_ERROR_BASE + 31);
    public const int  SP_ERR_FRAMEWORK_SDK_20 = (SP_SHELL_ERROR_BASE + 32);
    public const int  SP_ERR_APP_NOT_SUPPORTED = (SP_SHELL_ERROR_BASE + 33);
    public const int  SP_ERR_FILE_COPY = (SP_SHELL_ERROR_BASE + 34);
    public const int  SP_ERR_HEADER_SIZE_EXCEED = (SP_SHELL_ERROR_BASE + 35);
    public const int  SP_ERR_SGEN = (SP_SHELL_ERROR_BASE + 36);
    public const int  SP_ERR_CODE_MORPHING = (SP_SHELL_ERROR_BASE + 37);

   /* CMDShell Error codes*/
   public const int  SP_ERR_PARAMETER_MISSING = (SP_SHELL_ERROR_BASE + 50);
   public const int  SP_ERR_PARAMETER_IDENTIFIER_MISSING = (SP_SHELL_ERROR_BASE + 51);
   public const int  SP_ERR_PARAMETER_INVALID = (SP_SHELL_ERROR_BASE + 52);
   public const int  SP_ERR_REGISTRY = (SP_SHELL_ERROR_BASE + 54);
   public const int  SP_ERR_VERIFY_SIGN = (SP_SHELL_ERROR_BASE + 55);
   public const int  SP_ERR_PARAMETER = (SP_SHELL_ERROR_BASE + 56);
   public const int  SP_ERR_LICENSE_TEMPLATE_FILE = (SP_SHELL_ERROR_BASE + 57);
   public const int  SP_ERR_NO_DEVELOPER_KEY = (SP_SHELL_ERROR_BASE + 58);
   public const int  SP_ERR_NO_ENDUSER_KEY = (SP_SHELL_ERROR_BASE + 59);
   public const int  SP_ERR_NO_POINT_KEYS = (SP_SHELL_ERROR_BASE + 60);
   public const int  SP_ERR_NO_SHELL_FEATURE = (SP_SHELL_ERROR_BASE + 61);
   public const int  SP_ERR_SHELL_OPTION_FILE_MISSING = (SP_SHELL_ERROR_BASE + 62);
   public const int  SP_ERR_SHELL_OPTION_FILE_FORMAT = (SP_SHELL_ERROR_BASE +  63);
   public const int  SP_ERR_SHELL_OPTION_FILE_INVALID = (SP_SHELL_ERROR_BASE +  64);
   public const int  SP_ERR_DELETE_LICENSE = (SP_SHELL_ERROR_BASE +  65);
   public const int  SP_ERR_FILE_CREATE_FAILED = (SP_SHELL_ERROR_BASE +  66);
   public const int  SP_ERR_SHELLFILES_LIMIT = (SP_SHELL_ERROR_BASE +  67);
   public const int  SP_ERR_SINGLE_INSTANCE_ERROR = (SP_SHELL_ERROR_BASE +  68);
   public const int  SP_ERR_NO_EXE_FILE = (SP_SHELL_ERROR_BASE +  69);


	
	//Secure Update error codes
	public const int  SP_ERR_KEY_NOT_FOUND				= (SP_SECURE_UPDATE_ERROR_BASE + 1);
	public const int  SP_ERR_ILLEGAL_UPDATE				= (SP_SECURE_UPDATE_ERROR_BASE + 2);
	public const int  SP_ERR_DLL_LOAD_ERROR				= (SP_SECURE_UPDATE_ERROR_BASE + 3);
	public const int  SP_ERR_NO_CONFIG_FILE				= (SP_SECURE_UPDATE_ERROR_BASE + 4);
	public const int  SP_ERR_INVALID_CONFIG_FILE		= (SP_SECURE_UPDATE_ERROR_BASE + 5);
	public const int  SP_ERR_UPDATE_WIZARD_NOT_FOUND	= (SP_SECURE_UPDATE_ERROR_BASE + 6);
	public const int  SP_ERR_UPDATE_WIZARD_SPAWN_ERROR	= (SP_SECURE_UPDATE_ERROR_BASE + 7);
	public const int  SP_ERR_EXCEPTION_ERROR			= (SP_SECURE_UPDATE_ERROR_BASE + 8);
	public const int  SP_ERR_INVALID_CLIENT_LIB			= (SP_SECURE_UPDATE_ERROR_BASE + 9);
	public const int  SP_ERR_CABINET_DLL				= (SP_SECURE_UPDATE_ERROR_BASE + 10);
	public const int  SP_ERR_INSUFFICIENT_REQ_CODE_BUFFER = (SP_SECURE_UPDATE_ERROR_BASE + 11);
	public const int  SP_ERR_UPDATE_WIZARD_USER_CANCELED = (SP_SECURE_UPDATE_ERROR_BASE + 12);
	
    /* New Error codes defined for license addition*/
    public const int SP_ERR_INVALID_DLL_VERSION     = (SP_SECURE_UPDATE_ERROR_BASE + 13);
	public const int SP_ERR_INVALID_FILE_TYPE      =  (SP_SECURE_UPDATE_ERROR_BASE + 14); 
	
	public const int  SP_ERR_BAD_XML     =  (SP_SETUP_LIBRARY_ERROR_BASE + 1);
    public const int  SP_ERR_BAD_PACKET   = (SP_SETUP_LIBRARY_ERROR_BASE + 2);
    public const int  SP_ERR_BAD_FEATURE   = (SP_SETUP_LIBRARY_ERROR_BASE + 3);
    public const int  SP_ERR_BAD_HEADER     = (SP_SETUP_LIBRARY_ERROR_BASE + 4); 
    public const int  SP_ERR_ISV_MISSING     = (SP_SETUP_LIBRARY_ERROR_BASE + 5); 
    public const int  SP_ERR_DEVID_MISMATCH   = (SP_SETUP_LIBRARY_ERROR_BASE + 6);
    public const int  SP_ERR_LM_TOKEN_ERROR   = (SP_SETUP_LIBRARY_ERROR_BASE + 7);
    public const int  SP_ERR_LM_MISSING       = (SP_SETUP_LIBRARY_ERROR_BASE + 8);
    public const int  SP_ERR_INVALID_SIZE     = (SP_SETUP_LIBRARY_ERROR_BASE + 9);
    public const int  SP_ERR_FEATURE_NOT_FOUND   = (SP_SETUP_LIBRARY_ERROR_BASE + 10);
    public const int  SP_ERR_LICENSE_NOT_FOUND   =  (SP_SETUP_LIBRARY_ERROR_BASE + 11); 
    public const int  SP_ERR_BEYOND_RANGE        = (SP_SETUP_LIBRARY_ERROR_BASE + 12); 
	
	#endregion

	#region SentinelKey Constants values used by client application
	/* Set Protocol Flags */

	//SFNTGetLicense flags
	public const int SP_TCP_PROTOCOL					=	1;
	public const int SP_IPX_PROTOCOL					=	2;
	public const int SP_NETBEUI_PROTOCOL			    =	4;
	public const int SP_TCP6_PROTOCOL					=	8;
	public const int SP_STANDALONE_MODE				    =	32;
	public const int SP_SERVER_MODE						=	64;
	public const int SP_SHARE_ON						=	128;
	public const int SP_GET_NEXT_LICENSE                =   1024;
	public const int SP_ENABLE_TERMINAL_CLIENT			=   2048;

	//Query feature flag
	public const int SP_SIMPLE_QUERY					=	1;
	public const int SP_CHECK_DEMO						=	0;

	//Device Capabilities
	public const int SP_CAPS_AES_128					=	0x00000001;
	public const int SP_CAPS_ECC_K163					=	0x00000002;
	public const int SP_CAPS_ECC_KEYEXCH				=	0x00000004;
	public const int SP_CAPS_ECC_SIGN					=	0x00000008;
	public const int SP_CAPS_TIME_SUPP					=	0x00000010;
	public const int SP_CAPS_TIME_RTC					=	0x00000020;

	//Feature Attributies
	public const int SP_ATTR_WRITE_ONCE					=	0x0200;	
	public const int SP_ATTR_ACTIVE						=	0x0020;	
	public const int SP_ATTR_AUTODEC					=	0x0010;	
	public const int SP_ATTR_SIGN						=	0x0004;	
	public const int SP_ATTR_DECRYPT					=	0x0002;
	public const int SP_ATTR_ENCRYPT					=	0x0001;
	public const int SP_ATTR_SECMSG_READ				=	0x0080;
	
	//Feature Type
	public const int DATA_FEATURE_TYPE_BOOLEAN			=	1;
	public const int DATA_FEATURE_TYPE_BYTE				=	2;
	public const int DATA_FEATURE_TYPE_WORD				=	3;
	public const int DATA_FEATURE_TYPE_DWORD			=	4;
	public const int DATA_FEATURE_TYPE_RAW				=	5;
	public const int DATA_FEATURE_TYPE_STRING			=	6;
	public const int FEATURE_TYPE_COUNTER				=	7;
	public const int FEATURE_TYPE_AES					=	8;
	public const int FEATURE_TYPE_ECC					=	9;

	//Heartbeat Interval Scope
	public const int SP_MAX_HEARTBEAT					=	2592000;
	public const int SP_MIN_HEARTBEAT					=	60;
	public const uint SP_INFINITE_HEARTBEAT				=	0xFFFFFFFF;

	public const uint SP_PUBILC_KEY_LEN					=	42;
	public const uint SP_SOFTWARE_KEY_LEN				=	112;
	public const uint SP_MIN_ENCRYPT_DATA_LEN			=	16;
	public const uint SP_MAX_QUERY_LEN					=	112;
	public const uint SP_MAX_RAW_LEN					=	2032;
	public const uint SP_MAX_STRING_LEN					=	2032;
    public const uint SP_MAX_SIGN_BUFFER_LEN            =   0xFFFFFFFF;
	
	public const uint SP_MAX_NUM_SERVERS				=	100;
    
	
	//it is for SFNTEnumerateServer function
	public const ushort SP_RET_ON_FIRST_AVAILABLE		=	1;
	public const ushort SP_GET_ALL_SERVERS				=	4;

	#endregion

	#region  Server Info Structure Used by SFNTGetServerInfo
	public class ServerInfo
	{
		public byte[] bInfoBuffer;
		const uint MAX_SERVERINFO_LENGTH	= 70;
		const int BYTE_OFFSET_NAME			= 0;
		const int BYTE_OFFSET_PROTOCOLS		= 64;
		const int BYTE_OFFSET_MAJORVER		= 66;
		const int BYTE_OFFSET_MINORVER		= 68;
		
		System.Text.Encoding encoding = System.Text.Encoding.UTF8;

		public ServerInfo()
		{
			bInfoBuffer = new byte[MAX_SERVERINFO_LENGTH];
		}
		~ServerInfo()
		{
			bInfoBuffer = null;
		}
		public string ServerName
		{
			get
			{
				return encoding.GetString(this.bInfoBuffer,BYTE_OFFSET_NAME,64);
			}
		}
		public ushort Protocols
		{
			get
			{
				return BitConverter.ToUInt16(this.bInfoBuffer, BYTE_OFFSET_PROTOCOLS);
			}
		}
		public ushort MajorVersion
		{
			get
			{
				return BitConverter.ToUInt16(this.bInfoBuffer, BYTE_OFFSET_MAJORVER);
			}
		}
		public ushort MinorVersion
		{
			get
			{
				return BitConverter.ToUInt16(this.bInfoBuffer, BYTE_OFFSET_MINORVER);
			}
		}
	}
	#endregion

	#region  License Info Structure Used by SFNTGetLicenseInfo
	public class LicenseInfo
	{
		public byte[] bInfoBuffer;
		const uint MAX_LICENSEINFO_LENGTH	= 16;
		const int BYTE_OFFSET_LICENSEID		= 0;
		const int BYTE_OFFSET_USERLIMIT		= 4;
		const int BYTE_OFFSET_FEATURENUMS	= 8;
		const int BYTE_OFFSET_LICENSESIZE	= 12;
		public LicenseInfo()
		{
			bInfoBuffer = new byte[MAX_LICENSEINFO_LENGTH];
		}
		~LicenseInfo()
		{
			bInfoBuffer = null;
		}
		public uint LicenseID
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_LICENSEID); 
			}
		}
		public uint UserLimit
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_USERLIMIT);
			}
		}
		public uint FeatureNums
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_FEATURENUMS);
			}
		}		
		public uint LicenseSize
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_LICENSESIZE);
			}
		}
	}
	#endregion

	#region  Device Info Structure Used by SFNTGetDeviceInfo
	public class DeviceInfo
	{
		public byte[] bInfoBuffer;
		const uint MAX_DEVICEINFO_LENGTH		= 49;
		const int BYTE_OFFSET_FORMFACTORTYPE	= 0;
		const int BYTE_OFFSET_PRODUCTCODE		= 4;
		const int BYTE_OFFSET_HARDLIMIT			= 8;
		const int BYTE_OFFSET_CAPABILITIES		= 12;
		const int BYTE_OFFSET_DEVID				= 16;
		const int BYTE_OFFSET_DEVSN				= 20;
		const int BYTE_OFFSET_DEVYEAR			= 24;
		const int BYTE_OFFSET_DEVMONTH			= 28;
		const int BYTE_OFFSET_DEVDAY			= 29;
		const int BYTE_OFFSET_DEVHOUR			= 30;
		const int BYTE_OFFSET_DEVMINUTE			= 31;
		const int BYTE_OFFSET_DEVSECOND			= 32;
		const int BYTE_OFFSET_MEMSIZE			= 36;
		const int BYTE_OFFSET_FREESIZE			= 40;
		const int BYTE_OFFSET_DRVVERSION		= 44;
		public DeviceInfo()
		{
			bInfoBuffer = new byte[MAX_DEVICEINFO_LENGTH];
		}
		~DeviceInfo()
		{
			bInfoBuffer = null;
		}
		public uint FormFactorType
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_FORMFACTORTYPE); 
			}
		}
		public uint ProductCode
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_PRODUCTCODE);
			}
		}
		public uint Hardlimit
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_HARDLIMIT);
			}
		}		
		public uint Capabilities
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_CAPABILITIES);
			}
		}
		public uint DevID
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_DEVID); 
			}
		}
		public uint DevSN
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_DEVSN);
			}
		}
		public uint Year
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_DEVYEAR);
			}
		}		
		public byte Month
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_DEVMONTH];
			}
		}
		public byte Day
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_DEVDAY];
			}
		}
		public byte Hour
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_DEVHOUR];
			}
		}
		public byte Minute
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_DEVMINUTE];
			}
		}
		public byte Second
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_DEVSECOND];
			}
		}
		public uint MemorySize
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_MEMSIZE);
			}
		}
		public uint FreeSize
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_FREESIZE); 
			}
		}
		public uint DrvVersion
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_DRVVERSION);
			}
		}
	}
	#endregion

	#region  Feature Info Structure Used by SFNTGetFeatureInfo
	public class FeatureInfo
	{
		public byte[] bInfoBuffer;
		const int MAX_FEATUREINFO_LENGTH			= 36;
		const int BYTE_OFFSET_FEATURETYPE			= 0;
		const int BYTE_OFFSET_FEATURESIZE			= 4;
		const int BYTE_OFFSET_FEATUREATTRIBUTES		= 8;
		const int BYTE_OFFSET_ENABLECOUNTER			= 12;
		const int BYTE_OFFSET_ENABLESTOPTIME		= 13;
		const int BYTE_OFFSET_ENABLEDURATION		= 14;
		const int BYTE_OFFSET_DURATION				= 16;
		const int BYTE_OFFSET_FEATUREYEAR			= 20;
		const int BYTE_OFFSET_FEATUREMONTH			= 24;
		const int BYTE_OFFSET_FEATUREDAY			= 25;
		const int BYTE_OFFSET_FEATUREHOUR			= 26;
		const int BYTE_OFFSET_FEATUREMINUTE			= 27;
		const int BYTE_OFFSET_FEATURESECOND			= 28;
		const int BYTE_OFFSET_LEFTEXECUTIONNUMBER	= 32;
		public FeatureInfo()
		{
			bInfoBuffer = new byte[MAX_FEATUREINFO_LENGTH];
		}
		~FeatureInfo()
		{
			bInfoBuffer = null;
		}
		public uint FeatureType
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_FEATURETYPE); 
			}
		}
		public uint FeatureSize
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_FEATURESIZE);
			}
		}
		public uint FeatureAttributes
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_FEATUREATTRIBUTES);
			}
		}		
		public byte bEnableCounter
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_ENABLECOUNTER];
			}
		}
		public byte bEnableStopTime
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_ENABLESTOPTIME];
			}
		}
		public byte bEnableDurationTime
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_ENABLEDURATION];
			}
		}
		public uint Duration
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_DURATION); 
			}
		}
		public uint Year
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_FEATUREYEAR);
			}
		}		
		public byte Month
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_FEATUREMONTH];
			}
		}
		public byte Day
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_FEATUREDAY];
			}
		}
		public byte Hour
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_FEATUREHOUR];
			}
		}
		public byte Minute
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_FEATUREMINUTE];
			}
		}
		public byte Second
		{
			get
			{
				return this.bInfoBuffer[BYTE_OFFSET_FEATURESECOND];
			}
		}
		public uint LeftExecutionNumber
		{
			get
			{
				return BitConverter.ToUInt32(this.bInfoBuffer, BYTE_OFFSET_LEFTEXECUTIONNUMBER);
			}
		}
		
	}
	#endregion

	#region  Enum Server Info Structure Used by SFNTEnumServer
	public class EnumServerInfo
	{
		public byte[] bInfoBuffer;
		const int MAX_ENUMSERVERINFO_LENGTH			= 66;
		const int BYTE_OFFSET_SERVERADDR			= 0;
		const int BYTE_OFFSET_NUMLICAVAIL			= 64;

		System.Text.Encoding encoding = System.Text.Encoding.UTF8;
		public EnumServerInfo()
		{
			bInfoBuffer = new byte[MAX_ENUMSERVERINFO_LENGTH];
		}
		~EnumServerInfo()
		{
			bInfoBuffer = null;
		}
		public string ServerAddr
		{
			get
			{
				return encoding.GetString(this.bInfoBuffer,BYTE_OFFSET_SERVERADDR,64);
			}
		}
		public uint NumLicAvail
		{
			get
			{
				return BitConverter.ToUInt16(this.bInfoBuffer, BYTE_OFFSET_NUMLICAVAIL);
			}
		}
		
	}
	#endregion

	#region	//////////////////////// BEGIN PUBLIC METHODS	///////////////////////////////////
	public uint SFNTGetLicense(uint devID, byte[] softwareKey, uint licID, uint flags)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTGetLicense(devID, softwareKey, licID, flags, out this.licHandle); 
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}

	public uint SFNTReleaseLicense()
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTReleaseLicense(this.licHandle);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}
	
	public uint SFNTCounterDecrement(uint featureID, uint decrementValue)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTCounterDecrement(this.licHandle, featureID, decrementValue);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}

	public uint SFNTQueryFeature(uint featureID, uint flag, byte[] query, uint queryLength, byte[] response, uint responseLength)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTQueryFeature(this.licHandle, featureID, flag, query, queryLength, response, responseLength);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}

	public uint SFNTReadString(uint featureID, byte[] stringBuffer, uint stringLength)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTReadString(this.licHandle, featureID, stringBuffer, stringLength);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else			
		}

		return status;
	}

	public uint SFNTWriteString(uint featureID, byte[] stringBuffer,  uint writePassword)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTWriteString(this.licHandle, featureID, stringBuffer, writePassword);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}

	public uint SFNTReadInteger(uint featureID, out uint featureValue)
	{
		uint status = 0;
		featureValue = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTReadInteger(this.licHandle, featureID, out featureValue);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}

	public uint SFNTWriteInteger(uint featureID, uint featureValue,  uint writePassword)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTWriteInteger(this.licHandle, featureID, featureValue, writePassword);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}

	public uint SFNTReadRawData(uint featureID, byte[] rawDataBuffer,  uint offset,  uint length)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTReadRawData(this.licHandle, featureID, rawDataBuffer, offset, length);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}

	public uint SFNTWriteRawData(uint featureID, byte[] rawDataBuffer,  uint offset,  uint length, uint writePassword)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTWriteRawData(this.licHandle, featureID, rawDataBuffer, offset, length, writePassword);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}
	
	public uint SFNTEncrypt(uint featureID, byte[] plainBuffer, byte[] cipherBuffer)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTEncrypt(this.licHandle, featureID, plainBuffer, cipherBuffer);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}

	public uint SFNTDecrypt(uint featureID, byte[] cipherBuffer, byte[] plainBuffer)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTDecrypt(this.licHandle, featureID, cipherBuffer, plainBuffer);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}    
 
	public uint SFNTSign(uint featureID, byte[] signBuffer, uint length, byte[] signResult)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTSign(this.licHandle, featureID, signBuffer, length, signResult);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}  
 
	public uint SFNTVerify(byte[] publicKey, byte[] signBuffer,  uint length, byte[] signResult)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTVerify(this.licHandle, publicKey, signBuffer,length, signResult);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}  

	public uint SFNTSetHeartbeat(uint heartbeatValue)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTSetHeartbeat(this.licHandle, heartbeatValue);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}

	public uint SFNTGetLicenseInfo(SentinelKey.LicenseInfo licenseInfo)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTGetLicenseInfo(this.licHandle, licenseInfo.bInfoBuffer);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else			
		}

		return status;
	}

	public uint SFNTGetFeatureInfo(uint featureID, SentinelKey.FeatureInfo featureInfo)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTGetFeatureInfo(this.licHandle, featureID, featureInfo.bInfoBuffer);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else			
		}

		return status;
	}

	public uint SFNTGetDeviceInfo(SentinelKey.DeviceInfo deviceInfo)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTGetDeviceInfo(this.licHandle, deviceInfo.bInfoBuffer);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else			
		}

		return status;
	}
	
	public uint SFNTGetServerInfo(SentinelKey.ServerInfo serverInfo)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTGetServerInfo(this.licHandle, serverInfo.bInfoBuffer);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else			
		}

		return status;
	}
	
	public uint SFNTSetContactServer(byte[] serverAddr)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTSetContactServer(serverAddr);
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else			
		}

		return status;
	}

	public uint SFNTEnumServer(uint devID, uint licID, uint enumFlag, SentinelKey.EnumServerInfo[] srvInfo,ref ushort numSrvInfo)
	{
		uint status = 0;
		byte []buffer =new byte[numSrvInfo*66];

		try
		{
			status = SentinelKeyNativeAPI.SFNTEnumServer(devID,licID,enumFlag,buffer, ref numSrvInfo);
			for(int i=0;i<numSrvInfo;i++)
			{
				Array.Copy(buffer,i*66,srvInfo[i].bInfoBuffer,0,66);
			}
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else			
		}

		return status;
	}

	public uint SFNTSetConfigFile(byte[] configFilePath)
	{
		uint status = 0;

		try
		{
			status = SentinelKeyNativeAPI.SFNTSetConfigFile(configFilePath); 
		}
		catch (System.DllNotFoundException)
		{
			throw new System.DllNotFoundException(strError);
		}
		catch
		{
			// Anything else
		}

		return status;
	}
	#endregion //////////////////////// END PUBLIC METHODS	///////////////////////////////////
}