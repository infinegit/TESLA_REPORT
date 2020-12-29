using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I.Report.Base
{
    /// <summary>
    /// 图片上传 , 目前只支持单文件上传
    /// </summary>
    public class ImgUpload
    {
        /// <summary>
        /// 上传图片到服务器，包含域用户验证
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="targetImgPath"></param>
        /// <param name="selectFile"></param>
        /// <param name="strDomainName"></param>
        /// <param name="strDomainAccount"></param>
        /// <param name="strDomainPW"></param>
        /// <returns>iflag(1: 没有选择文件或文件大小为零，2：域用户验证错误)</returns>
        public int ImgUploadToServer
            (
                      string sourceFileName
                    , string targetImgPath
                    , HttpPostedFileBase selectFile

                    , string strDomainName
                    , string strDomainAccount
                    , string strDomainPW
            )
        {
            int iflag = 0;

            //验证上传文件
            var file = selectFile;
            if ( file.ContentLength == 0 )
            {
                iflag = 1;
            }
            else
            {
                //域用户验证
                DomainUserCheck duc = new DomainUserCheck();
                if ( duc.ValidateUserAccount(strDomainName , strDomainAccount , strDomainPW) )
                {
                    //生成缩略图
                    //保存文件到report 服务器中  
                    MakeThum mt = new MakeThum();

                    mt.MakeThumbnail(selectFile, targetImgPath, 500, 500, false);


                    //从 REPORT 服务器 保存文件到分公司 DB 服务器中  
                    //using ( FileStream fr = System.IO.File.OpenRead(strMapReport) )
                    //{
                    //    using ( FileStream fw = System.IO.File.OpenWrite(strMapCompany) )
                    //    {
                    //        //设置缓冲区大小
                    //        byte[] buffers = new byte[ 1024 * 1024 * 10 ];
                    //        //读取一次
                    //        int r = fr.Read(buffers , 0 , buffers.Length);
                    //        //判断本次是否读取到了数据
                    //        while ( r > 0 )
                    //        {
                    //            fw.Write(buffers , 0 , r);
                    //            r = fr.Read(buffers , 0 , buffers.Length);
                    //        }
                    //    }
                    //}

                    //删除report服务器中的文件
                    //System.IO.File.Delete(fileReportPath + strSaveFileName);
                }
                else
                {
                    iflag = 2;
                }
            }
            return iflag;
        }

        //此功能暂时没有用到
        //public ActionResult DeleteFlie(string strFilename)
        //{
        //    string FilePath = strFilename; //转换物理路径 
        //    if ( System.IO.File.Exists(FilePath) )//判断文件是否存在
        //    {
        //        System.IO.File.Delete(FilePath);//执行IO文件删除,需引入命名空间System.IO;    
        //    }
        //    return Json(new { OK = true });
        //}
    }
}