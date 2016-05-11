using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TeamDailyWork.Models;

namespace TeamDailyWork.Services
{
    /// <summary>
    /// 读写Xml文件的类
    /// </summary>
    public class XmlService
    {
        //XML文件地址
        private readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory + "WorkClassification.xml";
        //读取嵌入式资源
        Assembly _asm = Assembly.GetExecutingAssembly();

        /// <summary>
        /// 读取Xml文件内容
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<WorkClassification> ReadFromXml()
        {
            try
            {
                //如果程序目录下存在WorkClassification.xml文件中，就从硬盘中读取
                if (File.Exists(_basePath))
                {
                    using (FileStream fs = File.OpenRead(_basePath))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof (ObservableCollection<WorkClassification>));
                        ObservableCollection<WorkClassification> resultList =
                            xs.Deserialize(fs) as ObservableCollection<WorkClassification>;
                        return resultList;
                    }
                }
                else
                {
                    //如果程序目录下不存在此文件,就从嵌入的资源文件中读取,通常是在第一次使用程序时使用.
                    using (Stream sm = _asm.GetManifestResourceStream("TeamDailyWork.ColorsSetting.WorkClassification.xml"))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<WorkClassification>));
                        ObservableCollection<WorkClassification> resultList =
                            xs.Deserialize(sm) as ObservableCollection<WorkClassification>;
                        return resultList;
                    }
                }
            }
            catch (FileNotFoundException er)
            {
                return null;
            }
            catch (Exception err)
            {
                return null;
            }

        }


        /// <summary>
        /// 存储内容到Xml文件
        /// </summary>
        /// <param name="workClassification"></param>
        public void WriteToXml(ObservableCollection<WorkClassification> workClassification)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<WorkClassification>));
            using (FileStream fs = File.Open(_basePath, FileMode.Create))
            {
                xs.Serialize(fs, workClassification);
            }
        }
    }
}
