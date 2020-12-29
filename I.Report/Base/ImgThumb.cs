using System;
using System.Collections.Generic;
using System.Web;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

/// <SUMMARY>
///MakeThum 的摘要说明
/// </SUMMARY>
public class MakeThum
{
    public MakeThum()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    /// <SUMMARY>
    /// 生成缩略图//带压缩图片不压缩22k压缩2k
    /// </SUMMARY>
    /// <PARAM name="originalImagePath" />原始路径
    /// <PARAM name="thumbnailPath" />生成缩略图路径
    /// <PARAM name="width" />缩略图的宽
    /// <PARAM name="height" />缩略图的高
    //是否压缩图片质量
    public void MakeThumbnail(HttpPostedFileBase originalImageFile, string thumbnailPath, int width, int height, bool Ys)
    {
        //获取原始图片  
        System.Drawing.Image originalImage = System.Drawing.Image.FromStream(originalImageFile.InputStream);
        //缩略图画布宽高  
        int towidth = width;
        int toheight = height;
        //原始图片写入画布坐标和宽高(用来设置裁减溢出部分)  
        int x = 0;
        int y = 0;
        int ow = originalImage.Width;
        int oh = originalImage.Height;
        //原始图片画布,设置写入缩略图画布坐标和宽高(用来原始图片整体宽高缩放)  
        int bg_x = 0;
        int bg_y = 0;
        int bg_w = towidth;
        int bg_h = toheight;
        //倍数变量  
        double multiple = 0;
        //获取宽长的或是高长与缩略图的倍数  
        if (originalImage.Width >= originalImage.Height)
            multiple = (double)originalImage.Width / (double)width;
        else
            multiple = (double)originalImage.Height / (double)height;
        //上传的图片的宽和高小等于缩略图  
        if (ow <= width && oh <= height)
        {
            //缩略图按原始宽高  
            bg_w = originalImage.Width;
            bg_h = originalImage.Height;
            //空白部分用背景色填充  
            bg_x = Convert.ToInt32(((double)towidth - (double)ow) / 2);
            bg_y = Convert.ToInt32(((double)toheight - (double)oh) / 2);
        }
        //上传的图片的宽和高大于缩略图  
        else
        {
            //宽高按比例缩放  
            bg_w = Convert.ToInt32((double)originalImage.Width / multiple);
            bg_h = Convert.ToInt32((double)originalImage.Height / multiple);
            //空白部分用背景色填充  
            bg_y = Convert.ToInt32(((double)height - (double)bg_h) / 2);
            bg_x = Convert.ToInt32(((double)width - (double)bg_w) / 2);
        }
        //新建一个bmp图片,并设置缩略图大小.  
        System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
        //新建一个画板  
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
        //设置高质量插值法  
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
        //设置高质量,低速度呈现平滑程度  
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //清空画布并设置背景色  
        g.Clear(System.Drawing.ColorTranslator.FromHtml("#FFF"));
        //在指定位置并且按指定大小绘制原图片的指定部分  
        //第一个System.Drawing.Rectangle是原图片的画布坐标和宽高,第二个是原图片写在画布上的坐标和宽高,最后一个参数是指定数值单位为像素  
        g.DrawImage(originalImage, new System.Drawing.Rectangle(bg_x, bg_y, bg_w, bg_h), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);

        if (Ys)
        {

            System.Drawing.Imaging.ImageCodecInfo encoder = GetEncoderInfo("image/jpeg");
            try
            {
                if (encoder != null)
                {
                    System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters(1);
                    //设置 jpeg 质量为 60
                    encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)60);
                    bitmap.Save(thumbnailPath, encoder, encoderParams);
                    encoderParams.Dispose();

                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }

        }
        else
        {

            try
            {
                Bitmap bmp = new Bitmap(originalImageFile.InputStream);
                Image img = (Image)bmp;
                //新建第二个bitmap类型的bmp2变量，这里是根据程序需要设置的。
                Bitmap bmp2 = new Bitmap(1024, 768, PixelFormat.Format16bppRgb555);
                //将第一个bmp拷贝到bmp2中
                Graphics draw = Graphics.FromImage(bmp2);
                draw.DrawImage(bmp, 0, 0);
                draw.Dispose();
                bmp.Dispose();//释放bmp文件资源
                img.Dispose();//释放bmp文件资源
                Image img1 = (Image)bmp2;
                //获取图片类型  
                string fileExtension = System.IO.Path.GetExtension(originalImageFile.FileName).ToLower();
                //按原图片类型保存缩略图片,不按原格式图片会出现模糊,锯齿等问题.  
                switch (fileExtension)
                {
                    case ".gif": img1.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif); break;
                    case ".jpg": img1.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                    case ".bmp": img1.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp); break;
                    case ".png": img1.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png); break;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }

        }

    }
    private System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        //根据 mime 类型，返回编码器
        System.Drawing.Imaging.ImageCodecInfo result = null;
        System.Drawing.Imaging.ImageCodecInfo[] encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
        for (int i = 0; i < encoders.Length; i++)
        {
            if (encoders[i].MimeType == mimeType)
            {
                result = encoders[i];
                break;
            }

        }
        return result;

    }

}