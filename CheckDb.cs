using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;

namespace WeCode1._0
{
    public static class CheckDb
    {
        //判断数据库表是否存在最后更新时间字段，没有则新增字段，并且赋值创建时间
        public static void UpdateDB()
        {
            //检查updatetime字段
            CheckUpdatetime();

            //检查isLock字段
            CheckIsLock();

            //检查key
            CheckMyKeys();
        }

        //检查updatetime字段
        private static void CheckUpdatetime()
        {
            DataTable tempdt = AccessAdo.ExecuteDataSet("select * from ttree").Tables[0];
            Boolean isUpdateTimeExsist = false;
            for (int i = 0; i < tempdt.Columns.Count; i++)
            {
                if (tempdt.Columns[i].ColumnName == "UpdateTime")
                {
                    isUpdateTimeExsist = true;
                    break;
                }
                else
                {
                    isUpdateTimeExsist = false;
                }
            }

            if (isUpdateTimeExsist == false)
            {
                //不存在，添加字段，赋值
                string sql = "alter table ttree add COLUMN UpdateTime INTEGER";
                AccessAdo.ExecuteNonQuery(sql);
                sql = "update ttree set updatetime=createtime";
                AccessAdo.ExecuteNonQuery(sql);
            }
        }

        //检查isLock字段
        private static void CheckIsLock()
        {
            DataTable tempdt = AccessAdo.ExecuteDataSet("select * from ttree").Tables[0];
            Boolean isIsLockExsist = false;
            for (int i = 0; i < tempdt.Columns.Count; i++)
            {
                if (tempdt.Columns[i].ColumnName == "IsLock")
                {
                    isIsLockExsist = true;
                    break;
                }
                else
                {
                    isIsLockExsist = false;
                }
            }

            if (isIsLockExsist == false)
            {
                //不存在，添加字段，赋值
                string sql = "alter table ttree add COLUMN IsLock INTEGER DEFAULT 0";
                AccessAdo.ExecuteNonQuery(sql);
                sql = "update ttree set IsLock=0";
                AccessAdo.ExecuteNonQuery(sql);
            }
        }

        //检查秘钥表是否存在
        private static void CheckMyKeys()
        {
            //string sql = "select count(*) from MSysObjects where Name='MyKeys'";
            //int result = (int)AccessAdo.ExecuteScalar(sql);
            //if (result <= 0)
            //{
            //    sql = " CREATE TABLE MyKeys ( " +
            //    " [KeyE] MEMO, " +
            //    " [KeyD5] MEMO) ";
            //    AccessAdo.ExecuteNonQuery(sql);
            //}

            string sql = "select * from MyKeys";
            Boolean isExist = false;
            try
            {
                AccessAdo.ExecuteNonQuery(sql);
                isExist = true;
            }
            catch (Exception ex)
            {
                isExist = false;
            }

            if (isExist == false)
            {
                sql = " CREATE TABLE MyKeys ( " +
                " [KeyE] MEMO, " +
                " [KeyD5] MEMO) ";
                AccessAdo.ExecuteNonQuery(sql);
            }

        }

        //检查升级XML
        public static void CheckUpdateXML()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("TreeNodeLocal.xml");

            Boolean isExist=false;

            XmlNode xnode = xDoc.SelectSingleNode("//note[@IsLock]");
            if (xnode == null)
            {
                isExist = false;
            }
            else
            {
                isExist = true;
            }

            if (isExist == false)
            {
                XmlNodeList xLists = xDoc.SelectNodes("//note");
                foreach (XmlNode node in xLists)
                {
                    ((XmlElement)node).SetAttribute("IsLock", "0");
                }
            }

            xDoc.Save("TreeNodeLocal.xml");
            XMLAPI.XML2Yun();
        }

    }
}
