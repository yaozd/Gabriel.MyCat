using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Gabriel.MyCat.Message;
using Gabriel.MyCat.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gabriel.MyCat.Test
{
    [TestClass]
    public class CatTest
    {
        [TestMethod]
        public void Test_Logger()
        {
            var filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            const string fileName = "cat.log";
            Logger.Initialize(Path.Combine(filePath, fileName));
            Logger.Info("Cat .Net Client initialized.");
        }

        [TestMethod]
        public void Test_MyCatInstance()
        {
            var cat = MyCat.Instance;
        }

        [TestMethod]
        public void Test_CurrentThreadId()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// 事务
        /// </summary>
        [TestMethod]
        public void Test_NewTransaction()
        {
            Logger.TestWrite("initialized");
            const int upper = 1*10;
            for (int i = 0; i < upper; i++)
            {
                MyCat.Instance.NewTransaction("LY_Service", "Insure_DD");
                MyCat.Instance.Complete();
            }
        }

        /// <summary>
        /// 多线程下的上下文ID
        /// </summary>
        [TestMethod]
        public void Test_MultiThreadContext()
        {
            Logger.TestWrite("Test_MultiThreadContext");
            const int upper = 1*300;
            MyCat.Instance.NewTransaction("LY_Service", "Insure_DD");
            Logger.TestWrite("ManagedThreadId {0}", Thread.CurrentThread.ManagedThreadId);
            for (int i = 0; i < upper; i++)
            {
                Thread oThread = new Thread(new ThreadStart(MultiThreadContextTestMethod));
                oThread.Start();
            }
            Logger.TestWrite("ManagedThreadId {0}", Thread.CurrentThread.ManagedThreadId);
            MyCat.Instance.Complete();
        }

        private void MultiThreadContextTestMethod()
        {
            MyCat.Instance.NewTransaction("LY_Service", "Insure_DD");
            MyCat.Instance.Complete();
        }

        [TestMethod]
        public void Test_LogEven()
        {
            Logger.TestWrite("Test_LogEven");
            MyCat.Instance.NewTransaction("LY_Service", "Insure_DD");
            MyCat.Instance.LogEvent("LY_Even", "Even_Description");
            MyCat.Instance.Complete();
        }

        [TestMethod]
        public void Test_MultiTransaction()
        {
            Logger.TestWrite("Test_MultiTransaction");

            MyCat.Instance.NewTransaction("root_00000000", "root");
            MyCat.Instance.LogEvent("root_even_00000000", "");
            for (int i = 0; i < 1000; i++)
            {
                MultiTransactionTestMethod_1();

                MultiTransactionTestMethod_1();
            }
            MyCat.Instance.LogEvent("root_even_11111111", "");
            MyCat.Instance.Complete();

        }

        [TestMethod]
        public void Test_OutOfMemory()
        {
            Logger.TestWrite("Test_MultiTransaction");
            for (int j = 0; j < 2000000000; j++)
            {
                MyCat.Instance.NewTransaction("root_00000000", "root");
                MyCat.Instance.LogEvent("root_even_00000000", "");
                for (int i = 0; i < 1000; i++)
                {
                    MultiTransactionTestMethod_1();

                    MultiTransactionTestMethod_1();
                }
                MyCat.Instance.LogEvent("root_even_11111111", "");
                MyCat.Instance.Complete();
            }

        }

        public void MultiTransactionTestMethod_1()
        {
            MyCat.Instance.NewTransaction("child_T__T_111test2222", "");
            MyCat.Instance.Complete();
            MyCat.Instance.NewTransaction("child_T__T_111test1", "");
            MyCat.Instance.Complete();
            MyCat.Instance.NewTransaction("child_T__T_11111111", "");
            MyCat.Instance.Complete();
            MyCat.Instance.NewTransaction("child_T__T_11111111", "");
            MyCat.Instance.Complete();
            MyCat.Instance.NewTransaction("child_T_11111111", "");
            MyCat.Instance.LogEvent("child_T_even_1", "");

            MultiTransactionTestMethod_2();

            MyCat.Instance.LogEvent("child_T_even_2", "");
            MyCat.Instance.Complete();
        }

        public void MultiTransactionTestMethod_2()
        {
            MyCat.Instance.NewTransaction("child_T_222222", "");
            MyCat.Instance.LogEvent("child_T_even_222", "");
            MultiTransactionTestMethod_3();
            MyCat.Instance.LogEvent("child_T_even_222", "");
            MyCat.Instance.Complete();
        }

        public void MultiTransactionTestMethod_3()
        {
            MyCat.Instance.NewTransaction("child_T_222222", "");
            MyCat.Instance.LogEvent("child_T_even_222", "");
            MyCat.Instance.LogEvent("child_T_even_222", "");
            MyCat.Instance.Complete();
        }

        [TestMethod]
        public void Test_ExceptionTransaction()
        {
            Logger.TestWrite("Test_ExceptionTransaction");
            MyCat.Instance.NewTransaction("root_00000000", "root");
            MyCat.Instance.LogEvent("root_even_00000000", "");
            ExceptionTransaction_1();
            MyCat.Instance.LogEvent("root_even_11111111", "");
            MyCat.Instance.Complete();
        }

        public void ExceptionTransaction_1()
        {
            MyCat.Instance.NewTransaction("child_T_222222", "");
            MyCat.Instance.LogEvent("child_T_even_exception---222", "");
            try
            {
                throw new Exception("exception transaction!!");
            }
            catch (Exception ex)
            {
                MyCat.Instance.ExceptionEvent(ex.Message);
            }
            MyCat.Instance.LogEvent("child_T_even_222", "");
            MyCat.Instance.Complete();
        }

    }
}
