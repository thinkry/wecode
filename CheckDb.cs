using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Data.OleDb;

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

            UpdateTcontent();
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


        //将ttree表的最后更新时间移至tcontent表中
        private static void UpdateTcontent()
        {

            DataTable tempdt = AccessAdo.ExecuteDataSet("select * from tcontent").Tables[0];
            Boolean isContentUpdateTimeExsist = false;
            for (int i = 0; i < tempdt.Columns.Count; i++)
            {
                if (tempdt.Columns[i].ColumnName == "UpdateTime")
                {
                    isContentUpdateTimeExsist = true;
                    break;
                }
                else
                {
                    isContentUpdateTimeExsist = false;
                }
            }

            if (isContentUpdateTimeExsist == false)
            {
                //不存在，在tcontent中增加最后更新时间，默认值为创建时间
                string SQL = "alter table tcontent add COLUMN UpdateTime INTEGER";
                AccessAdo.ExecuteNonQuery(SQL);
                SQL = "update tcontent inner join ttree on tcontent.nodeid=ttree.nodeid set tcontent.updatetime=ttree.updatetime ";
                AccessAdo.ExecuteNonQuery(SQL);
            }

            tempdt = AccessAdo.ExecuteDataSet("select * from ttree").Tables[0];
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

            if (isUpdateTimeExsist == true)
            {
                //存在，删除该字段，并在tcontent中增加最后更新时间，默认值为创建时间
                string SQL = "alter table ttree drop COLUMN updatetime";
                AccessAdo.ExecuteNonQuery(SQL);
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

            //如果存在最后更新时间，则删除该字段
            Boolean isupdatetimeExist = false;

            XmlNode node1 = xDoc.SelectSingleNode("//note[@updatetime]");
            if (node1 == null)
            {
                isupdatetimeExist = false;
            }
            else
            {
                isupdatetimeExist = true;
            }

            if (isupdatetimeExist == true)
            {
                XmlNodeList xLists = xDoc.SelectNodes("//note");
                foreach (XmlNode node in xLists)
                {
                    ((XmlElement)node).RemoveAttribute("updatetime");
                }
            }
            xDoc.Save("TreeNodeLocal.xml");
            XMLAPI.XML2Yun();

            //检查youdao数据库文件是否存在，不存在则创建
            string path = @"db\youdao.mdb";

            if (!File.Exists(path)) //检查数据库是否已存在
            {
                //不存在，创建
                path = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path;
                // 创建一个CatalogClass对象的实例,
                ADOX.CatalogClass cat = new ADOX.CatalogClass();
                // 使用CatalogClass对象的Create方法创建ACCESS数据库
                cat.Create(path);

                //创建表，表结构在三个表中均新增gid字段，用于目录结构改变，nodeid变化时可以正确匹配到文章或者附件
                OleDbConnection conn = new OleDbConnection(path);
                string crtSQL = " CREATE TABLE TTree ( " +
                " [NodeId] INTEGER CONSTRAINT PK_TTree26 PRIMARY KEY, " +
                " [Title] VARCHAR, " +
                " [Path] VARCHAR, " +
                " [ParentId] INTEGER, " +
                " [Type] INTEGER, " +
                " [CreateTime] INTEGER, " +
                " [SynId] INTEGER, " +
                " [Turn] INTEGER,  " +
                " [MarkTime] INTEGER, " +
                " [IsLock] INTEGER DEFAULT 0 , " +
                " [Gid] VARCHAR ) ";
                AccessAdo.ExecuteNonQuery(conn, crtSQL);

                crtSQL = " CREATE TABLE TContent ( " +
                " [NodeId] INTEGER CONSTRAINT PK_TTree27 PRIMARY KEY, " +
                " [Content] MEMO, " +
                " [Note] MEMO, " +
                " [Link] MEMO, " +
                " [UpdateTime] INTEGER, " +
                " [Gid] VARCHAR, "+
                " [Path] VARCHAR, " +
                " [NeedSync] INTEGER DEFAULT 0) ";
                AccessAdo.ExecuteNonQuery(conn, crtSQL);

                crtSQL = " CREATE TABLE TAttachment ( " +
                " [AffixId] INTEGER CONSTRAINT PK_TTree28 PRIMARY KEY, " +
                " [NodeId] INTEGER, " +
                " [Title] VARCHAR, " +
                " [Data] IMAGE , " +
                " [Size] INTEGER, " +
                " [Time] INTEGER, " +
                " [Gid] VARCHAR ) ";
                AccessAdo.ExecuteNonQuery(conn, crtSQL);

                crtSQL = " CREATE TABLE MyKeys ( " +
                " [KeyE] MEMO, " +
                " [KeyD5] MEMO) ";
                AccessAdo.ExecuteNonQuery(conn, crtSQL);
            }
        }

    }
}
