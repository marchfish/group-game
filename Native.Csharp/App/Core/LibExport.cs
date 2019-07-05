/*
 *	�˴����� T4 ������� LibExport.tt ģ������, �������˽����´�����ô�, �����޸�!
 *	
 *	���ļ�������Ŀ Json �ļ����¼���������.
 */
using System;
using System.Runtime.InteropServices;
using System.Text;
using Native.Csharp.App.Event;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Interface;
using Native.Csharp.Sdk.Cqp;
using Native.Csharp.Sdk.Cqp.Model;
using Native.Csharp.Sdk.Cqp.Other;
using Unity;

namespace Native.Csharp.App.Core
{
    public class LibExport
    {
		#region --�ֶ�--
		private static Encoding _defaultEncoding = null;
		#endregion

		#region --���캯��--
		/// <summary>
		/// ��̬���캯��, ע������ע��ص�
		/// </summary>
		static LibExport ()
		{
			_defaultEncoding = Encoding.GetEncoding ("GB18030");
			
			// ��ʼ�� Costura.Fody
			CosturaUtility.Initialize ();
			
			// ��ʼ������ע������
			Common.UnityContainer = new UnityContainer ();

			// ����ʼ���÷�������ע��
			Event_AppMain.Registbackcall (Common.UnityContainer);

			// ע����ϵ��÷������зַ�
			Event_AppMain.Resolvebackcall (Common.UnityContainer);

			// �ַ�Ӧ�����¼�
			ResolveAppbackcall ();
		}
		#endregion
		
		#region --���ķ���--
		/// <summary>
		/// ���� AppID �� ApiVer, ��������ģ�����к�������Ŀ�����Զ���д AppID �� ApiVer
		/// </summary>
		/// <returns></returns>
		[DllExport (ExportName = "AppInfo", CallingConvention = CallingConvention.StdCall)]
		private static string AppInfo ()
		{
			// ���������޸�
			// 
			Common.AppName = "Ⱥ��Ϸ";
			Common.AppVersion = Version.Parse ("1.0.0");		

			//
			// ��ǰ��Ŀ����: com.guangyingart.demo
			// Api�汾: 9

			return string.Format ("{0},{1}", 9, "com.guangyingart.demo");
		}

		/// <summary>
		/// ���ղ�� AutoCode, ע���쳣
		/// </summary>
		/// <param name="authCode"></param>
		/// <returns></returns>
		[DllExport (ExportName = "Initialize", CallingConvention = CallingConvention.StdCall)]
		private static int Initialize (int authCode)
		{
			// ��Q��ȡӦ����Ϣ��������ܸ�Ӧ�ã���������������������AuthCode��
			Common.CqApi = new CqApi (authCode);

			// AuthCode ������Ϻ󽫶�����������й�, �Ա���������Ŀ�е���
			Common.UnityContainer.RegisterInstance<CqApi> (Common.CqApi);

			// ע����ȫ���쳣����ص�, ���ڲ���δ�������쳣, �ص��� ��Q ������
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			// ����������ֹ�����������κδ��룬���ⷢ���쳣���������ִ�г�ʼ����������Startup�¼���ִ�У�Type=1001����
			return 0;
		}
		#endregion
		
		#region --˽�з���--
		/// <summary>
		/// ȫ���쳣����, ���ڲ��񿪷���δ�������쳣, ���쳣���ص�����Q���д���
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			if (ex != null)
			{
				StringBuilder innerLog = new StringBuilder ();
				innerLog.AppendLine ("����δ�������쳣!");
				innerLog.AppendLine ("�쳣��ջ��");
				innerLog.AppendLine (ex.ToString ());
				Common.CqApi.AddFatalError (innerLog.ToString ());      //��δ���������쳣���ؿ�Q������
			}
		}
		
		/// <summary>
		/// ��ȡ���е�ע����, �ַ�����Ӧ���¼�
		/// </summary>
		private static void ResolveAppbackcall ()
		{
			/*
			 * Id: 2
			 * Name: Ⱥ��Ϣ����
			 */
			if (Common.UnityContainer.IsRegistered<IReceiveGroupMessage> ("Ⱥ��Ϣ����") == true)
			{
				ReceiveGroupMessage_2 = Common.UnityContainer.Resolve<IReceiveGroupMessage> ("Ⱥ��Ϣ����").ReceiveGroupMessage;
			}
			

		}
		#endregion
		
		#region --��������--
		/*
		 * Id: 2
		 * Type: 2
		 * Name: Ⱥ��Ϣ����
		 * Function: _eventGroupMsg
		 */
		public static event EventHandler<CqGroupMessageEventArgs> ReceiveGroupMessage_2;
		[DllExport (ExportName = "_eventGroupMsg", CallingConvention = CallingConvention.StdCall)]
		private static int Evnet__eventGroupMsg (int subType, int msgId, long fromGroup, long fromQQ, string fromAnonymous, IntPtr msg, int font)
		{
			GroupAnonymous anonymous = null;
			if (fromQQ == 80000000 && !string.IsNullOrEmpty (fromAnonymous))
			{
				anonymous = Common.CqApi.GetAnonymous (fromAnonymous);
			}
			CqGroupMessageEventArgs args = new CqGroupMessageEventArgs (2, msgId, fromGroup, fromQQ, anonymous, msg.ToString (_defaultEncoding));
			if (subType == 1)
			{
				if (ReceiveGroupMessage_2 != null)
				{
					ReceiveGroupMessage_2 (null, args);
				}
			}
			return Convert.ToInt32 (args.Handler);
		}


		#endregion
    }
}
