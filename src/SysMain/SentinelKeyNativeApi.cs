using System.Runtime.InteropServices;

public sealed class SentinelKeyNativeAPI
{
	#region  Import SentinelKey API from SentinelKey Dyanamic Library (SentinelKeyW.dll)

        [DllImport("SentinelKeyW.dll", 
                CharSet=CharSet.Ansi, 
                EntryPoint="SFNTGetLicense")]
        public static extern uint SFNTGetLicense( /* IN */ uint devID,
													/* IN */ byte[] softwareKey,
													/* IN */ uint licID,
													/* IN */ uint flags,
													/* OUT*/ out uint licHandle );
		
		[DllImport("SentinelKeyW.dll", 
				 CharSet=CharSet.Ansi, 
				 EntryPoint="SFNTQueryFeature")]
		public static extern uint SFNTQueryFeature( /* IN */ uint licHandle,
													/* IN */ uint featureID,
													/* IN */ uint flag,
													/* IN */ byte[] query,
													/* IN */ uint queryLength,
													/* OUT*/ byte[] response,
													/* IN */ uint responseLength );

		[DllImport("SentinelKeyW.dll", 
				CharSet=CharSet.Ansi, 
				EntryPoint="SFNTReadString")]
		public static extern uint SFNTReadString ( /* IN */ uint licHandle,
														/* IN */ uint featureID,
														/* OUT*/ byte[] stringBuffer, 
														/* IN */ uint stringLength );

		[DllImport("SentinelKeyW.dll", 
				CharSet=CharSet.Ansi, 
				EntryPoint="SFNTWriteString")]
		public static extern uint SFNTWriteString ( /* IN */ uint licHandle,
													/* IN */ uint featureID,
													/* IN */ byte[] stringBuffer, 
													/* IN */ uint writePassword );

		[DllImport("SentinelKeyW.dll", 
				CharSet=CharSet.Ansi, 
				EntryPoint="SFNTReadInteger")]
		public static extern uint SFNTReadInteger ( /* IN */ uint licHandle,
													/* IN */ uint featureID,
													/* OUT */ out uint featureValue );

		[DllImport("SentinelKeyW.dll", 
				CharSet=CharSet.Ansi, 
				EntryPoint="SFNTWriteInteger")]
		public static extern uint SFNTWriteInteger ( /* IN */ uint licHandle,
														/* IN */ uint featureID,
														/* IN */ uint featureValue, 
														/* IN */ uint writePassword );

		[DllImport("SentinelKeyW.dll", 
                CharSet=CharSet.Ansi, 
                EntryPoint="SFNTReadRawData")]
		public static extern uint SFNTReadRawData( /* IN */ uint licHandle,
													/* IN */ uint featureID,
													/* OUT*/ byte[] rawDataBuffer, 
													/* IN */ uint offset, 
													/* IN */ uint length );

		[DllImport("SentinelKeyW.dll", 
				CharSet=CharSet.Ansi, 
				EntryPoint="SFNTWriteRawData")]
		public static extern uint SFNTWriteRawData( /* IN */ uint licHandle,
													/* IN */ uint featureID,
													/* IN */ byte[] rawDataBuffer, 
													/* IN */ uint offset, 
													/* IN */ uint length,
													/* IN */ uint writePassword );
        [DllImport("SentinelKeyW.dll", 
                CharSet=CharSet.Ansi,
                EntryPoint="SFNTCounterDecrement")]
		public static extern uint SFNTCounterDecrement(/* IN */ uint licHandle,
														/* IN */ uint featureID,
                                                   /* IN */ uint decrementValue);

        [DllImport("SentinelKeyW.dll",
                CharSet=CharSet.Ansi,
                EntryPoint="SFNTEncrypt")]
        public static extern uint SFNTEncrypt(/* IN */ uint licHandle,
													/* IN */ uint featureID,
													/* IN */ byte[] plainBuffer,
                                               /* OUT*/ byte[] cipherBuffer );

		[DllImport("SentinelKeyW.dll",
				CharSet=CharSet.Ansi,
				EntryPoint="SFNTDecrypt")]
		public static extern uint SFNTDecrypt(/* IN */ uint licHandle,
												/* IN */ uint featureID,
												/* IN */ byte[] cipherBuffer,
												/* OUT*/ byte[] plainBuffer );    
 
		[DllImport("SentinelKeyW.dll",
				CharSet=CharSet.Ansi,
				EntryPoint="SFNTSign")]
		public static extern uint SFNTSign(/* IN */ uint licHandle,
												/* IN */ uint featureID,
												/* IN */ byte[] signBuffer,
												/* IN */ uint length,
												/* OUT*/ byte[] signResult );  
 
		[DllImport("SentinelKeyW.dll",
				CharSet=CharSet.Ansi,
				EntryPoint="SFNTVerify")]
		public static extern uint SFNTVerify(/* IN */ uint licHandle,
												/* IN */ byte[] publicKey,
												/* IN */ byte[] signBuffer,
												/* IN */ uint length,
												/* IN */ byte[] signResult );  

        [DllImport("SentinelKeyW.dll",
                CharSet=CharSet.Ansi, 
                EntryPoint="SFNTSetHeartbeat")]
        public static extern uint SFNTSetHeartbeat(/* IN */ uint licHandle,
                                                 /* IN */ uint heartbeatValue );

        [DllImport("SentinelKeyW.dll", 
                CharSet=CharSet.Ansi, 
                EntryPoint="SFNTGetLicenseInfo")]
        public static extern uint SFNTGetLicenseInfo(/* IN */ uint licHandle,
                                                   /* OUT*/ byte[] licenseInfo );

        [DllImport("SentinelKeyW.dll", 
                CharSet=CharSet.Ansi, 
                EntryPoint="SFNTGetFeatureInfo")]
        public static extern uint SFNTGetFeatureInfo(/* IN */ uint licHandle,
                                                   /* IN */ uint featureID,
                                                   /* OUT*/ byte[] featureInfo );


        [DllImport("SentinelKeyW.dll", 
                CharSet=CharSet.Ansi,
                EntryPoint="SFNTGetDeviceInfo")]
        public static extern uint SFNTGetDeviceInfo(/* IN */ uint licHandle,
                                                  /* OUT*/ byte[] deviceInfo );
	
        [DllImport("SentinelKeyW.dll", 
                CharSet=CharSet.Ansi, 
                EntryPoint="SFNTGetServerInfo")]
        public static extern uint SFNTGetServerInfo(/* IN */ uint licHandle,
														/* OUT*/ byte[] serverInfo );

        [DllImport("SentinelKeyW.dll", 
                CharSet=CharSet.Ansi, 
                EntryPoint="SFNTReleaseLicense")]
        public static extern uint SFNTReleaseLicense(/* IN */ uint licHandle);

		[DllImport("SentinelKeyW.dll", 
				CharSet=CharSet.Ansi, 
				EntryPoint="SFNTSetContactServer")]
		public static extern uint SFNTSetContactServer(/* IN */ byte[] serverAddr);

		[DllImport("SentinelKeyW.dll", 
				CharSet=CharSet.Ansi, 
				EntryPoint="SFNTEnumServer")]
		public static extern uint SFNTEnumServer(/* IN */ uint devID,
											 /* IN */ uint licID,
											 /* IN */ uint enumFlag,
											 /* IN */ byte[] srvInfo,
											 /* OUT */ref ushort numSrvInfo);
		[DllImport("SentinelKeyW.dll", 
			CharSet=CharSet.Ansi, 
			EntryPoint="SFNTSetConfigFile")]
			public static extern uint SFNTSetConfigFile ( /* IN */ byte[] configFilePath);

       
        #endregion
}