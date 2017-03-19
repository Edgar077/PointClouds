using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTKExtension
{
    public class Texture : IDisposable
    {
        #region Propreties
        public string Filename { get; private set; }

        public int TextureID;
        public Bitmap BitmapTexture;
        
        public Size Size { get; private set; }

        public TextureTarget TextureTarget { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a texture from the supplied filename.
        /// Any files that Bitmap.FromFile can open are supported.
        /// This method also supports dds textures (as long as the file extension is .dds).
        /// </summary>
        /// <param name="Filename">The path to the texture to load.</param>
        public Texture(string Filename)
        {
            if (!File.Exists(Filename))
            {
                throw new FileNotFoundException(string.Format("The file {0} does not exist.", Filename));
            }

            this.Filename = Filename;
            switch (new FileInfo(Filename).Extension.ToLower())
            {
              
                default: LoadBitmap((Bitmap)Bitmap.FromFile(Filename));
                    break;
            }

            
        }

        /// <summary>
        /// Create a texture from a supplie bitmap.
        /// </summary>
        /// <param name="BitmapImage">The already decoded bitmap image.</param>
        /// <param name="FlipY">True if the bitmap should be flipped.</param>
        public Texture(Bitmap BitmapImage, bool FlipY = true)
        {
            this.Filename = BitmapImage.GetHashCode().ToString();
           
            LoadBitmap(BitmapImage, FlipY);
            
        }

        #endregion

        #region Methods

      

        private void LoadBitmap(Bitmap myBitmapImage, bool FlipY = true)
        {
        
            this.BitmapTexture = myBitmapImage;
            if (FlipY) BitmapTexture.RotateFlip(RotateFlipType.RotateNoneFlipY);     // bitmaps read from bottom up, so flip it
            this.Size = BitmapTexture.Size;
            
        }
        public void InitGL(bool hasAlpha)
        {
            this.TextureID = GL.GenTexture();
            // this is expensive, so we really only want to do it once...
           
            this.TextureTarget = TextureTarget.Texture2D; // set the texture target 
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1); // set pixel alignment
            //GL.PixelStore(PixelStoreParameter.PackAlignment, 1); // set pixel alignment

            GL.BindTexture(this.TextureTarget, TextureID);     // bind the texture to memory in OpenGL


            System.Drawing.Imaging.BitmapData bitmapData; 
            //convert to a bitmap.  Then the bitmap is locked into memory so
            //that the garbage collector doesn't touch it, and it is read via OpenGL glTexImage2D. *
            if (hasAlpha)
            {
               
                bitmapData = BitmapTexture.LockBits
                    (
                    new System.Drawing.Rectangle(0, 0, BitmapTexture.Width, BitmapTexture.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );

            }
            else
            {
                bitmapData = BitmapTexture.LockBits(
                    new System.Drawing.Rectangle(0, 0, BitmapTexture.Width, BitmapTexture.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb
                );
            }



           
           // GL.TexImage2D(this.TextureTarget, 0, PixelInternalFormat.Rgba8, this.Size.Width, this.Size.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, bitmapData.Scan0);

          
            // Defines filter used when texture is scaled (during the drawing)
            GL.TexParameter(this.TextureTarget, TextureParameterName.TextureMagFilter, Convert.ToInt32(TextureParameter.Nearest)); // MAG_FILTER = magnified filter : when the texture is enlarged
            GL.TexParameter(this.TextureTarget, TextureParameterName.TextureMinFilter, Convert.ToInt32(TextureParameter.Nearest)); // MIN_FILTER = minimized filter : when the texture is shrinked

            //set 2D Image to texture: bitmapData
            GL.TexImage2D(this.TextureTarget, 0, PixelInternalFormat.Rgba8, this.Size.Width, this.Size.Height,
                0, //0 : no border
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,   // why is this Bgr when the lockbits is rgb!? This cost me one day of stupid research throgh all the OpenGL needed and unneded calls...
                PixelType.UnsignedByte, bitmapData.Scan0);  //picture data



            BitmapTexture.UnlockBits(bitmapData);
            BitmapTexture.Dispose();

            
            GL.BindTexture(this.TextureTarget, 0);

        }
        //public void InitGL_Old(bool hasAlpha)
        //{
        //    this.TextureID = GL.GenTexture();
        //    this.TextureTarget = TextureTarget.Texture2D;
        //    GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1); // set pixel alignment
          
        //    System.Drawing.Imaging.BitmapData bitmapData;
        //    if (hasAlpha)
        //    {
        //        bitmapData = BitmapImage.LockBits
        //            (
        //            new System.Drawing.Rectangle(0, 0, BitmapImage.Width, BitmapImage.Height),
        //            System.Drawing.Imaging.ImageLockMode.ReadOnly,
        //            System.Drawing.Imaging.PixelFormat.Format32bppArgb
        //        );

        //    }
        //    else
        //    {
        //        bitmapData = BitmapImage.LockBits(
        //            new System.Drawing.Rectangle(0, 0, BitmapImage.Width, BitmapImage.Height),
        //            System.Drawing.Imaging.ImageLockMode.ReadOnly,
        //            System.Drawing.Imaging.PixelFormat.Format24bppRgb
        //        );
        //    }


        //    //Code to get the data to the OpenGL Driver

        //    GL.Enable(EnableCap.Texture2D);
        //    GL.ActiveTexture(TextureUnit.Texture0);

        //    //tell OpenGL that this is a 2D texture
        //    GL.BindTexture(this.TextureTarget, TextureID);

        //    //the following code sets certian parameters for the texture
        //    // GL.TexEnv (TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Combine);
        //    // GL.TexEnv (TextureEnvTarget.TextureEnv, TextureEnvParameter.CombineRgb, (float)TextureEnvMode.Modulate);

        //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
        //    // this assumes mipmaps are present...
        //    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);

     
        //    // tell OpenGL to build mipmaps out of the bitmap data
        //    // .. what a mess ... http://www.g-truc.net/post-0256.html
        //    // this is the old way, must be called before texture is loaded, see below for new way...
        //    // GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, (float)1.0f);

        //    // tell openGL the next line begins on a word boundary...
        //    GL.PixelStore(PixelStoreParameter.UnpackAlignment, 4);

        //    // load the texture
        //    if (hasAlpha)
        //    {
        //        GL.TexImage2D(
        //            TextureTarget.Texture2D,            //target : usually GL_TEXTURE_2D
        //            0,                                  //level : usually left to zero
        //            PixelInternalFormat.Rgba,           //format - RGBA 
        //            BitmapImage.Width, BitmapImage.Height,   //image size
        //            0,                                  //0 : no border
        //            PixelFormat.Bgra,                   // why is this Bgr when the lockbits is rgb!?
        //            PixelType.UnsignedByte,             //data type : pixels are made of byte
        //            bitmapData.Scan0                     //picture datas
        //        );
        //        //Console.WriteLine("SSTexture: loaded alpha ({0},{1}) from: {2}", TextureBitmap.Width, TextureBitmap.Height, name);
        //    }
        //    else
        //    {
        //        GL.TexImage2D(
        //            TextureTarget.Texture2D,
        //            0, // level
        //            PixelInternalFormat.Rgb,
        //            BitmapImage.Width, BitmapImage.Height,
        //            0, // border
        //            PixelFormat.Bgr,     // why is this Bgr when the lockbits is rgb!?
        //            PixelType.UnsignedByte,
        //            bitmapData.Scan0
        //        );
        //        //Console.WriteLine("SSTexture: loaded ({0},{1}) from: {2}", TextureBitmap.Width, TextureBitmap.Height, name);
        //    }

            

        //    //free the bitmap data (we dont need it anymore because it has been passed to the OpenGL driver
        //    BitmapImage.UnlockBits(bitmapData);		 

        //    //-------------------
        //    // set the texture target and then generate the texture ID
        //   // this.TextureTarget = TextureTarget.Texture2D;




        //   //GL.TexImage2D(TextureTarget, 0, PixelInternalFormat.Rgba8, this.Size.Width, this.Size.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, bitmapData.Scan0);

        //   // GL.TexParameter(TextureTarget, TextureParameterName.TextureMagFilter, Convert.ToInt32(TextureParameter.Nearest));

        //   // GL.TexParameter(TextureTarget, TextureParameterName.TextureMinFilter, Convert.ToInt32(TextureParameter.Nearest));//(int)TextureParam.Linear);   // linear filter



        //   // BitmapImage.UnlockBits(bitmapData);
        //   // BitmapImage.Dispose();



        //    GL.BindTexture(this.TextureTarget, 0);

        //}
        /// <summary>
        /// Loads a compressed DDS file into an OpenGL texture.
        /// </summary>
        /// <param name="ResourceFile">The path to the DDS file.</param>
  
        #endregion

        public void Dispose()
        {
            if (TextureID != 0)
            {
                GL.DeleteTexture(TextureID);
                TextureID = 0;
            }
        }
        ~Texture()
        {
            if (TextureID != 0) System.Diagnostics.Debug.Fail(string.Format("Texture {0} was not disposed of properly.", Filename));
        }
    }


    public enum PixelFormat_OpenGL : int
    {
        ColorIndex = ((int)0x1900),
        StencilIndex = ((int)0x1901),
        DepthComponent = ((int)0x1902),
        Red = ((int)0x1903),
        Green = ((int)0x1904),
        Blue = ((int)0x1905),
        Alpha = ((int)0x1906),
        Rgb = ((int)0x1907),
        Rgba = ((int)0x1908),
        Luminance = ((int)0x1909),
        LuminanceAlpha = ((int)0x190A),
        AbgrExt = ((int)0x8000),
        CmykExt = ((int)0x800C),
        CmykaExt = ((int)0x800D),
        Bgr = ((int)0x80E0),
        Bgra = ((int)0x80E1),
        Ycrcb422Sgix = ((int)0x81BB),
        Ycrcb444Sgix = ((int)0x81BC),
        Rg = ((int)0x8227),
        RgInteger = ((int)0x8228),
        DepthStencil = ((int)0x84F9),
        RedInteger = ((int)0x8D94),
        GreenInteger = ((int)0x8D95),
        BlueInteger = ((int)0x8D96),
        AlphaInteger = ((int)0x8D97),
        RgbInteger = ((int)0x8D98),
        RgbaInteger = ((int)0x8D99),
        BgrInteger = ((int)0x8D9A),
        BgraInteger = ((int)0x8D9B),
    }
    public enum TextureParameter : int
    {
        Nearest = ((int)0x2600),
        Linear = ((int)0x2601),
        NearestMipMapNearest = ((int)0x2700),
        LinearMipMapNearest = ((int)0x2701),
        NearestMipMapLinear = ((int)0x2702),
        LinearMipMapLinear = ((int)0x2703),
        ClampToEdge = ((int)0x812F),
        ClampToBorder = ((int)0x812D),
        MirrorClampToEdge = ((int)0x8743),
        MirroredRepeat = ((int)0x8370),
        Repeat = ((int)0x2901),
        Red = ((int)0x1903),
        Green = ((int)0x1904),
        Blue = ((int)0x1905),
        Alpha = ((int)0x1906),
        Zero = ((int)0),
        One = ((int)1),
        CompareRefToTexture = ((int)0x884E),
        None = ((int)0),
        StencilIndex = ((int)0x1901),
        DepthComponent = ((int)0x1902)
    }
}
