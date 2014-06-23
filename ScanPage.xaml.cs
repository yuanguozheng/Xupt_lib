using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices;
using com.google.zxing;
using com.google.zxing.oned;
using com.google.zxing.qrcode;
using com.google.zxing.common;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Xupt_lib
{
    public partial class ScanPage : PhoneApplicationPage
    {
        private PhotoCamera _photoCamera;
        private PhotoCameraLuminanceSource _luminance;
        private readonly DispatcherTimer _timer;
        private Reader _reader = null;

        public ScanPage()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(250);
            _timer.Tick += (o, arg) => ScanPreviewBuffer();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (Microsoft.Devices.Environment.DeviceType == Microsoft.Devices.DeviceType.Emulator)
            {
                MessageBox.Show("请勿使用模拟器！");
                this.IsEnabled = false;
                base.NavigationService.GoBack();
            }
            else
            {
                string type = "";
                if (NavigationContext.QueryString.TryGetValue("type", out type) && type == "qrcode")
                {
                    _reader = new QRCodeReader();
                }
                else
                {
                    _reader = new EAN13Reader();
                }

                _photoCamera = new PhotoCamera();
                
                    _photoCamera.Initialized += new EventHandler<CameraOperationCompletedEventArgs>(cam_Initialized);
                _videoBrush.SetSource(_photoCamera);
                BarCodeRectInitial();
                base.OnNavigatedTo(e);
            }
            frame.Tap += frame_Tap;
        }

        void frame_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (_photoCamera != null)
            {
                if (_photoCamera.IsFocusAtPointSupported == true)
                {
                    //try
                    //{
                    //    // Determine the location of the tap.
                    //    Point tapLocation = e.GetPosition(frame);

                    //    // Position the focus brackets with the estimated offsets.
                    //    focusBrackets.SetValue(Canvas.LeftProperty, tapLocation.X - 30);
                    //    focusBrackets.SetValue(Canvas.TopProperty, tapLocation.Y - 28);

                    //    // Determine the focus point.
                    //    double focusXPercentage = tapLocation.X / frame.Width;
                    //    double focusYPercentage = tapLocation.Y / frame.Height;

                    //    // Show the focus brackets and focus at point.
                    //    focusBrackets.Visibility = Visibility.Visible;
                    //    _photoCamera.FocusAtPoint(focusXPercentage, focusYPercentage);

                    //    // Write a message to the UI.
                    //    this.Dispatcher.BeginInvoke(delegate()
                    //    {
                    //        //txtDebug.Text = String.Format("Camera focusing at point: {0:N2} , {1:N2}", focusXPercentage, focusYPercentage);
                    //    });
                    //}
                    //catch (Exception focusError)
                    //{
                    //    // Cannot focus when a capture is in progress.
                    //    this.Dispatcher.BeginInvoke(delegate()
                    //    {
                    //        // Write a message to the UI.
                    //        //txtDebug.Text = focusError.Message;
                    //        // Hide focus brackets.
                    //        focusBrackets.Visibility = Visibility.Collapsed;
                    //    });
                    //}
                    
                    _photoCamera.Focus();
                }
                else
                {
                    // Write a message to the UI.
                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        //txtDebug.Text = "Camera does not support FocusAtPoint().";
                    });
                }

               /* if (_photoCamera.IsFocusAtPointSupported == true)
                {
                    try
                    {
                        // Determine the location of the tap.
                        Point tapLocation = e.GetPosition(frame);

                        // Position the focus brackets with the estimated offsets.
                        //focusBrackets.SetValue(Canvas.LeftProperty, tapLocation.X - 30);
                        //focusBrackets.SetValue(Canvas.TopProperty, tapLocation.Y - 28);

                        // Determine the focus point.
                        double focusXPercentage = tapLocation.X / frame.Width;
                        double focusYPercentage = tapLocation.Y / frame.Height;

                        // Show the focus brackets and focus at point.
                        //focusBrackets.Visibility = Visibility.Visible;
                        _photoCamera.FocusAtPoint(focusXPercentage, focusYPercentage);

                        // Write a message to the UI.
                        //this.Dispatcher.BeginInvoke(delegate()
                        //{
                        //    //txtDebug.Text = String.Format("Camera focusing at point: {0:N2} , {1:N2}", focusXPercentage, focusYPercentage);
                        //});
                    }
                    catch (Exception focusError)
                    {
                        //// Cannot focus when a capture is in progress.
                        //this.Dispatcher.BeginInvoke(delegate()
                        //{
                        //    // Write a message to the UI.
                        //    txtDebug.Text = focusError.Message;
                        //    // Hide focus brackets.
                        //    focusBrackets.Visibility = Visibility.Collapsed;
                        //});
                    }
                }
                else
                {
                    // Write a message to the UI.
                    //this.Dispatcher.BeginInvoke(delegate()
                    //{
                    //    txtDebug.Text = "Camera does not support FocusAtPoint().";
                    //});
                }*/
            }
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (_photoCamera != null)
            {
                _timer.Stop();
                _photoCamera.CancelFocus();
                _photoCamera.Dispose();
            }

            base.OnNavigatingFrom(e);
        }

        void cam_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            int width = Convert.ToInt32(_photoCamera.PreviewResolution.Width);
            int height = Convert.ToInt32(_photoCamera.PreviewResolution.Height);
            _luminance = new PhotoCameraLuminanceSource(width, height);

            Dispatcher.BeginInvoke(() =>
            {
                _previewTransform.Rotation = _photoCamera.Orientation;
                _timer.Start();
            });
            _photoCamera.FlashMode = FlashMode.Auto;
            
            _photoCamera.Focus();
        }

        public void SetStillPicture()
        {
            int width = Convert.ToInt32(960);
            int height = Convert.ToInt32(1280);
            int[] PreviewBuffer = new int[width * height];
            _photoCamera.GetPreviewBufferArgb32(PreviewBuffer);

            WriteableBitmap wb = new WriteableBitmap(width, height);
            PreviewBuffer.CopyTo(wb.Pixels, 0);

            MemoryStream ms = new MemoryStream();
            wb.SaveJpeg(ms, wb.PixelWidth, wb.PixelHeight, 0, 80);
            ms.Seek(0, SeekOrigin.Begin);

            BitmapImage bi = new BitmapImage();
            bi.SetSource(ms);
            ImageBrush still = new ImageBrush();
            still.ImageSource = bi;
            frame.Fill = still;
            still.RelativeTransform = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = _photoCamera.Orientation };
        }

        private void ScanPreviewBuffer()
        {
            try
            {
                //_photoCamera.Focus();
                _photoCamera.GetPreviewBufferY(_luminance.PreviewBufferY);
                var binarizer = new HybridBinarizer(_luminance);
                var binBitmap = new BinaryBitmap(binarizer);
                Result result = _reader.decode(binBitmap);
                if (result != null)
                {
                    _timer.Stop();
                    SetStillPicture();
                    BarCodeRectSuccess();
                    Dispatcher.BeginInvoke(() =>
                    {
                        //读取成功，结果存放在result.Text
                        //NavigationService.Navigate(new Uri("/ScanResult.xaml?result=" + result.Text, UriKind.Relative));
                        App.SearchParam = result.Text;
                        string uri = string.Format("/SearchResult.xaml?type={0}&from=barcode", "number");
                        this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
                        
                    });
                }
                else
                {
                    _photoCamera.Focus();
                }
            }
            catch
            {
            }
        }

        void BarCodeRectSuccess()
        {
            Dispatcher.BeginInvoke(() =>
            {
                _marker1.Fill = new SolidColorBrush(Colors.Green);
                _marker2.Fill = new SolidColorBrush(Colors.Green);
                _marker3.Fill = new SolidColorBrush(Colors.Green);
                _marker4.Fill = new SolidColorBrush(Colors.Green);
                _marker5.Fill = new SolidColorBrush(Colors.Green);
                _marker6.Fill = new SolidColorBrush(Colors.Green);
                _marker7.Fill = new SolidColorBrush(Colors.Green);
                _marker8.Fill = new SolidColorBrush(Colors.Green);
            });
        }

        void BarCodeRectInitial()
        {
            _marker1.Fill = new SolidColorBrush(Colors.Red);
            _marker2.Fill = new SolidColorBrush(Colors.Red);
            _marker3.Fill = new SolidColorBrush(Colors.Red);
            _marker4.Fill = new SolidColorBrush(Colors.Red);
            _marker5.Fill = new SolidColorBrush(Colors.Red);
            _marker6.Fill = new SolidColorBrush(Colors.Red);
            _marker7.Fill = new SolidColorBrush(Colors.Red);
            _marker8.Fill = new SolidColorBrush(Colors.Red);
        }

         
    }
}