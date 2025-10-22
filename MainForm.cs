
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace CircularImageGenerator
{
    public partial class MainForm : Form
    {
        private readonly Color originalButtonColor;
        private Point originalButtonLocation;

        private readonly Color originalConvertButtonColor;
        private Point originalConvertButtonLocation;

        // THÊM MỚI: Danh sách đường dẫn file ảnh đầu vào cho Tab Converter
        private readonly List<string> inputImagePathsICO = new List<string>();

        public MainForm()
        {
            // Khởi tạo components để tránh lỗi null khi tạo Timer
            this.components = new System.ComponentModel.Container();

            InitializeComponent();
            // Thiết lập giá trị mặc định (đảm bảo các control NumericUpDown có tên đúng)
            numStartAngle.Minimum = -360;
            numStartAngle.Maximum = 360;
            numStartAngle.Value = 0;

            numEndAngle.Minimum = -360;
            numEndAngle.Maximum = 360;
            numEndAngle.Value = 360;

            // KHAI BÁO numStepAngle (Tên biến phải khớp với Designer)
            numStepAngle.Minimum = 1;
            numStepAngle.Maximum = 360;
            numStepAngle.Value = 1; // Mặc định 1 độ

            // Lưu màu gốc của nút Generate
            originalButtonColor = BtnGenerate.BackColor;
            originalButtonLocation = BtnGenerate.Location; // THÊM MỚI

            // THÊM MỚI: Lưu màu và vị trí gốc cho nút Convert ICO
            originalConvertButtonColor = btnConvertICO.BackColor;
            originalConvertButtonLocation = btnConvertICO.Location;

            // THÊM: Thay đổi màu cho các label cũ (label1, label2, label3) để nổi bật
            label1.ForeColor = Color.DarkBlue; // Màu cho "Start Angle"
            label2.ForeColor = Color.DarkBlue; // Màu cho "End Angle"
            label3.ForeColor = Color.DarkBlue; // Màu cho "Step Angle"

            // THÊM MỚI: Thiết lập cho cmbIconSize (thay vì numIconSize)
            cmbIconSize.Items.AddRange(new string[] { "16x16", "32x32", "48x48", "64x64", "128x128", "256x256" });
            cmbIconSize.SelectedIndex = 1; // Mặc định chọn 32x32

            // THÊM MỚI: Thiết lập DrawMode cho TabControl
            tabControlMain.DrawMode = TabDrawMode.OwnerDrawFixed;

            // THÊM MỚI: Gọi hàm điều chỉnh kích thước tab
            AdjustTabSizes();
        }

        // =================================================================
        //                 PHẦN XỬ LÝ ẢNH (LOGIC CƠ BẢN)
        // =================================================================

        // Xoay ảnh gốc một góc nhất định quanh tâm, tính toán kích thước mới
        // để đảm bảo ảnh không bị cắt góc và tâm xoay luôn đồng nhất.
        public Image RotateImage(Image img, float angle)
        {
            float w = img.Width;
            float h = img.Height;

            // SỬA LỖI: Chỉ cần lấy phần dư cho an toàn, hoặc không cần làm gì vì GDI+ hỗ trợ góc lớn.
            // Chúng ta giữ lại logic này để đảm bảo hàm lượng giác (cos, sin) luôn tính toán trong phạm vi nhỏ.
            angle %= 360; // <== ĐÃ SỬA: Loại bỏ phép toán modulo kép gây lỗi chia cho 0.

            // Chuyển góc sang Radian
            double radians = (double)angle * Math.PI / 180.0;
            double cos = Math.Abs(Math.Cos(radians));
            double sin = Math.Abs(Math.Sin(radians));

            // 1. Tính toán kích thước mới đủ lớn để chứa toàn bộ ảnh đã xoay
            int newWidth = (int)Math.Round(w * cos + h * sin);
            int newHeight = (int)Math.Round(w * sin + h * cos);

            // 2. Tạo Bitmap mới với kích thước đã tính và nền trong suốt
            Bitmap rotatedImage = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppPArgb);
            rotatedImage.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Tính toán tâm của ảnh mới và ảnh cũ
                PointF centerNew = new PointF(newWidth / 2f, newHeight / 2f);
                PointF centerOld = new PointF(w / 2f, h / 2f);

                // Dịch chuyển tâm của khung ảnh mới (newImage) về gốc (0,0)
                g.TranslateTransform(centerNew.X, centerNew.Y);

                // Xoay ảnh
                g.RotateTransform(angle);

                // Dịch chuyển ngược lại để vẽ ảnh cũ (img) vào tâm khung ảnh mới
                // (centerOld) chính là điểm vẽ ảnh gốc
                g.TranslateTransform(-centerOld.X, -centerOld.Y);

                // 4. Vẽ ảnh gốc lên Graphics object đã biến đổi
                g.DrawImage(img, new Point(0, 0));
            }

            return rotatedImage;
        }

        public Image CropToCircle(Image originalImage, int fixedDiameter) // ĐÃ SỬA: Thêm fixedDiameter
        {
            // Sử dụng đường kính cố định thay vì tính toán lại từ kích thước ảnh đã xoay
            int diameter = fixedDiameter;

            // originalImage là ảnh đã xoay (rotatedImage), có kích thước lớn hơn ảnh gốc
            // Tọa độ x, y phải được tính từ tâm của ảnh đã xoay (rotatedImage)
            int x = (originalImage.Width - diameter) / 2;
            int y = (originalImage.Height - diameter) / 2;

            Bitmap croppedImage = new Bitmap(diameter, diameter, PixelFormat.Format32bppPArgb);
            croppedImage.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

            using (Graphics g = Graphics.FromImage(croppedImage))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, diameter, diameter);
                    g.SetClip(path);

                    // Vẽ phần ảnh đã xoay vào vùng clip hình tròn
                    g.DrawImage(originalImage, new Rectangle(0, 0, diameter, diameter),
                                               new Rectangle(x, y, diameter, diameter),
                                               GraphicsUnit.Pixel);
                }
            }
            return croppedImage;
        }

        // =================================================================
        //                 XỬ LÝ SỰ KIỆN GIAO DIỆN ROTATOR
        // =================================================================
        private void BtnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                // Chỉ cho phép chọn file ảnh
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtImagePath.Text = ofd.FileName;

                    //Hiện ảnh xem trước khi chọn xong
                    picPreview.Image = Image.FromFile(ofd.FileName);
                    picPreview.SizeMode = PictureBoxSizeMode.Zoom; // Để ảnh fit vào PictureBox mà không méo
                }
            }
        }

        private void BtnSelectOutput_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = fbd.SelectedPath;
                }
            }
        }

        // Nút tạo ảnh 
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            // THÊM HIỆU ỨNG "NHẤN LÚN SÂU"
            BtnGenerate.BackColor = Color.DarkGreen; // Màu "nhấn"
            int sinkAmount = 3; // Di chuyển xuống dưới 5 pixels
            BtnGenerate.Location = new Point(originalButtonLocation.X, originalButtonLocation.Y + sinkAmount);
            timerButtonEffect.Start();

            // 1. LẤY GIÁ TRỊ VÀ KIỂM TRA THAM SỐ GÓC
            int startAngle = (int)numStartAngle.Value;
            int endAngle = (int)numEndAngle.Value;
            int stepAngle = (int)numStepAngle.Value;

            if (startAngle > endAngle)
            {
                UpdateStatus("Lỗi: Góc Bắt Đầu phải nhỏ hơn hoặc bằng Góc Kết Thúc.", Color.Red);
                return;
            }
            if (stepAngle <= 0)
            {
                UpdateStatus("Lỗi: Bước nhảy góc phải lớn hơn 0.", Color.Red);
                return;
            }

            // 2. KIỂM TRA ĐƯỜNG DẪN
            if (!File.Exists(txtImagePath.Text))
            {
                UpdateStatus("Lỗi: Vui lòng chọn ảnh đầu vào hợp lệ.", Color.Red);
                return;
            }
            if (!Directory.Exists(txtOutputPath.Text))
            {
                UpdateStatus("Lỗi: Vui lòng chọn thư mục xuất hợp lệ.", Color.Red);
                return;
            }

            string inputPath = txtImagePath.Text;
            string outputDir = txtOutputPath.Text;

            // Bắt đầu xử lý
            UpdateStatus("Đang tạo ảnh...", Color.Blue);
            BtnGenerate.Enabled = false;

            // Xóa preview cũ
            if (picPreview.Image != null)
            {
                picPreview.Image.Dispose();
                picPreview.Image = null;
            }

            Image originalImage = null; // Khai báo ngoài try-catch để sử dụng trong finally

            try
            {
                originalImage = Image.FromFile(inputPath);

                // TÍNH TOÁN ĐƯỜNG KÍNH CỐ ĐỊNH (FIXED DIAMETER)
                int fixedDiameter = Math.Min(originalImage.Width, originalImage.Height);

                // Tính toán tổng số ảnh sẽ tạo
                int totalImages = (int)Math.Floor((decimal)(endAngle - startAngle) / stepAngle) + 1;
                int imageCounter = 0; // Biến đếm số lượng ảnh thực tế được tạo

                // Vòng lặp
                for (int angle = startAngle, i = 1; angle <= endAngle; angle += stepAngle, i++)
                {
                    // Cập nhật trạng thái chi tiết
                    UpdateStatus($"Đang xử lý góc: {angle}° ({i} / {totalImages})", Color.Blue);

                    // Xóa ảnh preview cũ nếu có
                    if (picPreview.Image != null)
                    {
                        picPreview.Image.Dispose();
                        picPreview.Image = null;
                    }

                    // Thao tác xoay và cắt tròn
                    using (Image rotatedImage = RotateImage(originalImage, angle))
                    using (Image circularImage = CropToCircle(rotatedImage, fixedDiameter))
                    {
                        // THÊM PREVIEW MỚI: Clone ảnh để hiển thị (tránh dispose ảnh gốc)
                        picPreview.Image = (Image)circularImage.Clone();
                        picPreview.SizeMode = PictureBoxSizeMode.Zoom;

                        // Cập nhật UI ngay lập tức
                        Application.DoEvents();

                        // Lưu ảnh
                        string fileName = $"icon_{i}.png";
                        string outputPath = Path.Combine(outputDir, fileName);
                        circularImage.Save(outputPath, ImageFormat.Png);

                        imageCounter++;
                    }

                    // THÊM MỚI: Delay 20ms để tránh quá tải
                    Thread.Sleep(20);
                }

                // THÔNG BÁO THÀNH CÔNG
                UpdateStatus($"Hoàn thành! Đã tạo ra {imageCounter} ảnh thành công.", Color.Green);

            }
            catch (Exception ex)
            {
                // XỬ LÝ LỖI NGOẠI LỆ
                UpdateStatus($"Lỗi trong quá trình xử lý: {ex.Message}", Color.Red);
            }
            finally
            {
                BtnGenerate.Enabled = true;

                // Dọn dẹp ảnh gốc đã tải
                originalImage?.Dispose();
            }
        }

        // THÊM MỚI: Hàm cập nhật status với màu
        private void UpdateStatus(string message, Color color)
        {
            labelStatus.Text = message;
            labelStatus.ForeColor = color;
        }

        // Event Timer để reset màu và vị trí nút sau hiệu ứng
        private void TimerButtonEffect_Tick(object sender, EventArgs e)
        {
            timerButtonEffect.Stop();
            BtnGenerate.BackColor = originalButtonColor; // Quay về màu gốc
            BtnGenerate.Location = originalButtonLocation; // Quay về vị trí gốc

            // THÊM MỚI: Reset cho nút Convert ICO nếu cần (nếu timer dùng chung)
            btnConvertICO.BackColor = originalConvertButtonColor;
            btnConvertICO.Location = originalConvertButtonLocation;
        }     

        // =================================================================
        //                 XỬ LÝ SỰ KIỆN GIAO DIỆN CONVERTER
        // =================================================================
        private void BtnSelectImageICO_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files (*.*)|*.*";
                ofd.Multiselect = true; // Cho phép chọn nhiều file
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // LƯU DANH SÁCH FILE VÀ CẬP NHẬT TEXTBOX
                    inputImagePathsICO.Clear();
                    inputImagePathsICO.AddRange(ofd.FileNames);

                    // Xóa preview cũ
                    picPreviewICO.Image = null;

                    if (inputImagePathsICO.Count > 0)
                    {
                        string firstFilePath = inputImagePathsICO[0];

                        // HIỂN THỊ FILE ĐẦU TIÊN LÀM PREVIEW (Dù chọn 1 hay nhiều file)
                        try
                        {
                            picPreviewICO.Image = Image.FromFile(firstFilePath);
                            picPreviewICO.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                        catch (Exception ex)
                        {
                            UpdateStatusICO($"Lỗi tải ảnh preview ({Path.GetFileName(firstFilePath)}): {ex.Message}", Color.OrangeRed);
                            // Giữ picPreviewICO.Image là null nếu tải lỗi
                        }

                        if (inputImagePathsICO.Count == 1)
                        {
                            txtInputImageICO.Text = firstFilePath;
                            UpdateStatusICO("Sẵn sàng chuyển đổi 1 file.", Color.DarkGreen);
                        }
                        else
                        {
                            txtInputImageICO.Text = $"Đã chọn {inputImagePathsICO.Count} file ảnh (Preview: {Path.GetFileName(firstFilePath)})...";
                            UpdateStatusICO($"Sẵn sàng chuyển đổi {inputImagePathsICO.Count} file.", Color.DarkGreen);
                        }
                    }
                }
            }
        }

        private void BtnSelectOutputICO_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog()) // THAY ĐỔI: Dùng FolderBrowserDialog
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputICO.Text = fbd.SelectedPath; // Lưu đường dẫn thư mục
                }
            }
        }

        private void BtnConvertICO_Click(object sender, EventArgs e)
        {
            // THÊM HIỆU ỨNG "NHẤN LÚN SÂU" cho nút Convert ICO
            btnConvertICO.BackColor = Color.DarkGreen;
            int sinkAmount = 3;
            btnConvertICO.Location = new Point(originalConvertButtonLocation.X, originalConvertButtonLocation.Y + sinkAmount);
            timerButtonEffect.Start();

            // KIỂM TRA ĐẦU VÀO
            if (inputImagePathsICO.Count == 0)
            {
                UpdateStatusICO("Lỗi: Vui lòng chọn ít nhất một ảnh đầu vào hợp lệ.", Color.Red);
                return;
            }
            if (!Directory.Exists(txtOutputICO.Text))
            {
                UpdateStatusICO("Lỗi: Vui lòng chọn thư mục output hợp lệ.", Color.Red);
                return;
            }

            btnConvertICO.Enabled = false;

            string outputDir = txtOutputICO.Text;
            int convertedCount = 0;
            int totalFiles = inputImagePathsICO.Count;
            // THÊM MỚI: Biến để lưu đường dẫn file ICO cuối cùng
            string lastOutputFilePath = string.Empty;

            // Lấy kích thước icon từ ComboBox
            string selectedSize = cmbIconSize.SelectedItem.ToString();
            int iconSize = int.Parse(selectedSize.Split('x')[0]);

            try
            {
                foreach (string inputPath in inputImagePathsICO)
                {
                    convertedCount++;
                    string originalFileName = Path.GetFileNameWithoutExtension(inputPath);
                    string outputFilePath = Path.Combine(outputDir, originalFileName + ".ico");

                    // LƯU ĐƯỜNG DẪN CUỐI CÙNG TRƯỚC KHI XỬ LÝ
                    lastOutputFilePath = outputFilePath;

                    // 1. CẬP NHẬT TRẠNG THÁI VÀ PREVIEW CHO ẢNH GỐC
                    UpdateStatusICO($"Đang chuyển đổi: {originalFileName} ({convertedCount} / {totalFiles})", Color.Blue);

                    // Xóa ảnh preview cũ nếu có
                    if (picPreviewICO.Image != null)
                    {
                        picPreviewICO.Image.Dispose();
                        picPreviewICO.Image = null;
                    }

                    // HIỂN THỊ ẢNH GỐC ĐỂ PREVIEW TRƯỚC KHI CONVERT
                    Image originalImageForPreview = null;
                    try
                    {
                        originalImageForPreview = Image.FromFile(inputPath);
                        picPreviewICO.Image = originalImageForPreview;
                        picPreviewICO.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch (Exception ex)
                    {
                        UpdateStatusICO($"Lỗi tải ảnh preview ({originalFileName}): {ex.Message} - Vẫn đang chuyển đổi...", Color.OrangeRed);
                    }

                    Application.DoEvents();

                    // 2. THỰC HIỆN CONVERT
                    // Load lại ảnh gốc (hoặc dùng lại biến đã dùng cho preview nếu bạn chắc chắn nó không bị Dispose)
                    // Ở đây tôi dùng lại biến originalImageForPreview để tối ưu, nhưng cần đảm bảo nó không null
                    using (Image img = originalImageForPreview ?? Image.FromFile(inputPath))
                    {
                        // Resize ảnh với chất lượng cao
                        using (Bitmap resized = ResizeImage(img, iconSize, iconSize))
                        {
                            // Lưu dưới dạng ICO với PNG nhúng để giữ chất lượng
                            SaveAsIcon(resized, outputFilePath);
                        }
                    }

                    // Dọn dẹp ảnh gốc đã dùng để preview
                    originalImageForPreview?.Dispose();

                    Thread.Sleep(20); // Delay nhỏ
                }

                // THÔNG BÁO THÀNH CÔNG CUỐI CÙNG
                UpdateStatusICO($"Hoàn thành! Đã convert thành công {totalFiles} file.", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatusICO($"Lỗi trong quá trình xử lý: {ex.Message}", Color.Red);
            }
            finally
            {
                btnConvertICO.Enabled = true;

                // LOGIC MỚI: HIỂN THỊ ICON CUỐI CÙNG ĐÃ TẠO
                if (totalFiles > 0 && File.Exists(lastOutputFilePath))
                {
                    // Đảm bảo ảnh preview cũ (nếu có) được Dispose trước
                    if (picPreviewICO.Image != null)
                    {
                        picPreviewICO.Image.Dispose();
                        picPreviewICO.Image = null;
                    }

                    try
                    {
                        // Icon là một loại Image đặc biệt, cần dùng Icon class để tải
                        using (Icon icon = new Icon(lastOutputFilePath))
                        {
                            // Chuyển Icon sang Bitmap để hiển thị trong PictureBox và giữ tham chiếu
                            picPreviewICO.Image = icon.ToBitmap();
                            picPreviewICO.SizeMode = PictureBoxSizeMode.CenterImage; // Icon nên để CenterImage hoặc Zoom
                        }
                    }
                    catch (Exception ex)
                    {
                        // Lỗi nếu không tải được ICO (thường là file hỏng)
                        UpdateStatusICO($"Hoàn thành, nhưng lỗi tải preview file cuối cùng: {ex.Message}", Color.Orange);
                    }
                }
                // Nếu không có file nào được xử lý, để trống preview
                else if (picPreviewICO.Image != null)
                {
                    picPreviewICO.Image.Dispose();
                    picPreviewICO.Image = null;
                }
            }
        }

        // THÊM MỚI: Hàm cập nhật status cho ICO tab
        private void UpdateStatusICO(string message, Color color)
        {
            lblStatusICO.Text = message;
            lblStatusICO.ForeColor = color;
        }

        // THÊM MỚI: Hàm resize ảnh với chất lượng cao
        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        // <summary>
        // Lưu Bitmap dưới dạng ICO với PNG nhúng để giữ chất lượng (hỗ trợ 256x256)
        // </summary>
        private void SaveAsIcon(Bitmap bitmap, string filePath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Lưu dưới dạng PNG để đảm bảo độ trong suốt và chất lượng tốt nhất
                bitmap.Save(ms, ImageFormat.Png);
                byte[] pngData = ms.ToArray();

                // Viết ICO header
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    // Phần đầu ICO: 6 bytes
                    bw.Write((short)0); // Dành riêng (Reserved)
                    bw.Write((short)1); // Kiểu (1 là cho ICO)
                    bw.Write((short)1); // Số lượng ảnh

                    // Mục nhập thư mục Icon (IconDirEntry): 16 bytes
                    byte width = (byte)(bitmap.Width == 256 ? 0 : bitmap.Width); // 0 đại diện cho kích thước 256x256
                    byte height = (byte)(bitmap.Height == 256 ? 0 : bitmap.Height); // 0 đại diện cho kích thước 256x256
                    bw.Write(width);
                    bw.Write(height);
                    bw.Write((byte)0); // Số màu (0 cho >256 màu)
                    bw.Write((byte)0); // Dành riêng (Reserved)
                    bw.Write((short)1); // Số mặt phẳng màu (Planes)
                    bw.Write((short)32); // Số bit màu (32-bit)
                    bw.Write((int)pngData.Length); // Kích thước dữ liệu PNG
                    bw.Write((int)22); // Vị trí bắt đầu dữ liệu (Offset) (header 6 + mục nhập 16 = 22)

                    // Viết PNG data
                    bw.Write(pngData);
                }
            }
        }

        // <summary>
        // Hàm xử lý vẽ lại TabControl để đổi màu tiêu đề của tab đang được chọn và tăng kích thước chữ.
        // </summary>
        private void TabControlMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (!(sender is TabControl tabCtl)) return;

            TabPage tabPage = tabCtl.TabPages[e.Index];
            string tabText = tabPage.Text;

            // Màu nền và chữ
            _ = SystemColors.Control; // Màu nền tab không được chọn
            _ = SystemColors.ControlText; // Màu chữ tab không được chọn

            // KHAI BÁO FONT - Chỉ in đậm, không tăng kích thước
            Font fontToDispose = null;
            Font fontToUse = tabCtl.Font;
            Color backColor;
            Color foreColor;
            // Nếu đây là tab đang được chọn, thay đổi màu và font
            if (e.State == DrawItemState.Selected)
            {
                // Màu nền và chữ nổi bật cho tab được chọn
                backColor = System.Drawing.ColorTranslator.FromHtml("#F4A460"); // Màu xanh nhạt nổi bật
                foreColor = Color.DarkBlue; // Màu xanh đậm cho chữ

                // Chỉ in đậm, không tăng kích thước font
                fontToDispose = new Font(tabCtl.Font.FontFamily, tabCtl.Font.Size, FontStyle.Bold);
                fontToUse = fontToDispose;
            }
            else
            {
                // Màu cho tab không được chọn
                backColor = SystemColors.Control; // Màu xám nhạt
                foreColor = SystemColors.GrayText; // Màu chữ xám
            }

            // 1. Vẽ nền tab
            using (SolidBrush backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            // 2. Vẽ viền xung quanh tab để phân biệt rõ hơn
            using (Pen borderPen = new Pen(SystemColors.ControlDark))
            {
                //e.Graphics.DrawRectangle(borderPen, e.Bounds);
            }

            // 3. Vẽ chữ tiêu đề - SỬA: Hiển thị đầy đủ text
            using (SolidBrush foreBrush = new SolidBrush(foreColor))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    // QUAN TRỌNG: Không cắt text, không giới hạn dòng
                    FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip,
                    Trimming = StringTrimming.None // Không cắt bớt text
                };

                // Vẽ chữ trực tiếp trong bounds gốc
                e.Graphics.DrawString(tabText, fontToUse, foreBrush, e.Bounds, sf);
            }

            // Quan trọng: Giải phóng đối tượng Font tùy chỉnh nếu nó đã được tạo.
            fontToDispose?.Dispose();
        }
        private void AdjustTabSizes()
        {
            try
            {
                int maxWidth = 0;
                using (Graphics g = tabControlMain.CreateGraphics())
                {
                    // Tính chiều rộng lớn nhất cần thiết
                    foreach (TabPage tab in tabControlMain.TabPages)
                    {
                        SizeF textSize = g.MeasureString(tab.Text, tabControlMain.Font);
                        int requiredWidth = (int)textSize.Width + 20; // Padding 20 pixels
                        if (requiredWidth > maxWidth)
                        {
                            maxWidth = requiredWidth;
                        }
                    }

                    // Giới hạn chiều rộng tối đa (tuỳ chọn)
                    int maxAllowedWidth = 150;
                    if (maxWidth > maxAllowedWidth)
                    {
                        maxWidth = maxAllowedWidth;
                    }

                    // Đảm bảo chiều rộng tối thiểu
                    int minWidth = 80;
                    if (maxWidth < minWidth)
                    {
                        maxWidth = minWidth;
                    }

                    // Áp dụng kích thước mới
                    if (maxWidth > 0)
                    {
                        tabControlMain.ItemSize = new Size(maxWidth, tabControlMain.ItemSize.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine("Lỗi khi điều chỉnh kích thước tab: " + ex.Message);
            }
        }
    }
}

