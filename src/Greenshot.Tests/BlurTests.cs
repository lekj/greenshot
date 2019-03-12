﻿#region Greenshot GNU General Public License

// Greenshot - a free and open source screenshot tool
// Copyright (C) 2007-2018 Thomas Braun, Jens Klingen, Robin Krom
// 
// For more information see: http://getgreenshot.org/
// The Greenshot project is hosted on GitHub https://github.com/greenshot/greenshot
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 1 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Greenshot.Gfx;
using Greenshot.Gfx.Experimental;
using Greenshot.Gfx.Experimental.Structs;
using Greenshot.Tests.Implementation;
using Xunit;

namespace Greenshot.Tests
{
    /// <summary>
    /// This tests if the new blur works
    /// </summary>
    public class BlurTests
    {
        [Theory]
        [InlineData(PixelFormat.Format24bppRgb)]
        [InlineData(PixelFormat.Format32bppRgb)]
        [InlineData(PixelFormat.Format32bppArgb)]
        public void Test_Blur(PixelFormat pixelFormat)
        {
            using (var bitmapNew = BitmapFactory.CreateEmpty(400, 400, pixelFormat, Color.White))
            using (var bitmapOld = BitmapFactory.CreateEmpty(400, 400, pixelFormat, Color.White))
            {
                using (var graphics = Graphics.FromImage(bitmapNew))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                    bitmapNew.ApplyBoxBlur(10);
                }
                using (var graphics = Graphics.FromImage(bitmapOld))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                    bitmapOld.ApplyOldBoxBlur(10);
                }
                bitmapOld.Save(@"old.png", ImageFormat.Png);
                bitmapNew.Save(@"new.png", ImageFormat.Png);

                Assert.True(bitmapOld.IsEqualTo(bitmapNew), "New blur doesn't compare to old.");
            }
        }

        [Theory]
        [InlineData(PixelFormat.Format24bppRgb)]
        [InlineData(PixelFormat.Format32bppRgb)]
        [InlineData(PixelFormat.Format32bppArgb)]
        public void Test_Blur_Span(PixelFormat pixelFormat)
        {
            using (var bitmapNew = BitmapFactory.CreateEmpty(400, 400, pixelFormat, Color.White))
            using (var bitmapOld = BitmapFactory.CreateEmpty(400, 400, pixelFormat, Color.White))
            {
                using (var graphics = Graphics.FromImage(bitmapNew))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                    bitmapNew.ApplyBoxBlurSpan(10);
                }
                using (var graphics = Graphics.FromImage(bitmapOld))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                    bitmapOld.ApplyOldBoxBlur(10);
                }
                bitmapOld.Save(@"old.png", ImageFormat.Png);
                bitmapNew.Save(@"new.png", ImageFormat.Png);

                Assert.True(bitmapOld.IsEqualTo(bitmapNew), "New blur doesn't compare to old.");
            }
        }

        [Fact]
        public void Test_Blur_UnmanagedBitmap()
        {
            using (var bitmapNew = new UnmanagedBitmap<Bgr32>(400, 400))
            using (var bitmapOld = BitmapFactory.CreateEmpty(400, 400, PixelFormat.Format32bppRgb, Color.White))
            {
                using (var graphics = Graphics.FromImage(bitmapOld))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                    //bitmapOld.ApplyOldBoxBlur(10);
                    bitmapOld.ApplyBoxBlurSpan(10);
                }
                bitmapOld.Save(@"old.png", ImageFormat.Png);

                bitmapNew.Span.Fill(new Bgr32 { B = 255, G = 255, R = 255 });
                using (var bitmap = bitmapNew.AsBitmap())
                using (var graphics = Graphics.FromImage(bitmap))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                    bitmapNew.ApplyBoxBlur(10);
                    bitmap.Save(@"new.png", ImageFormat.Png);
                }

                Assert.True(bitmapOld.IsEqualTo(bitmapNew.AsBitmap()), "New blur doesn't compare to old.");
            }
        }
    }
}
