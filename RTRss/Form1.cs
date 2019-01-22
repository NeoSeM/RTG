using Argotic.Syndication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTRss
{
    public partial class Form1 : Form
    {
        private ImageList _images;
        public Form1()
        {
            InitializeComponent();
            _images = new ImageList()
            {
                ImageSize = new Size(64, 64),
                ColorDepth = ColorDepth.Depth32Bit
            };
            listView1.LargeImageList = _images;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadListView();    
        }

        private void LoadListView()
        {
            var feed = RssFeed.Create(new Uri(@"http://alt.rutor.info/rss.php?full=8"));
            foreach (RssItem post in feed.Channel.Items)
            {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(post.Description);
                var imageUrl = doc.DocumentNode.SelectNodes("//img").First().Attributes["src"].Value;

                var image = LoadImage(imageUrl);
                //image = ResizeImage(image, 64, 64, true);

                _images.Images.Add(post.Link.AbsoluteUri, image);
                var item = new ListViewItem(post.Title)
                {
                    Tag = post,
                    ImageKey = post.Link.AbsoluteUri
                };
                listView1.Items.Add(item);
            }
        }

        private void RefreshList()
        {
            webBrowser1.DocumentText = string.Empty;
            webBrowser1.Navigate("about:blank");
            webBrowser1.Refresh();
            listView1.Clear();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            webBrowser1.Navigate("about:blank");
            if (listView1.SelectedItems.Count > 0)
            {
                var rssItem = listView1.SelectedItems[0].Tag as RssItem;
                webBrowser1.Document.Write(rssItem.Description);
                webBrowser1.Refresh();
            }

        }
        private Image LoadImage(string url)
        {
            System.Net.WebRequest request =
                System.Net.WebRequest.Create(url);

            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream =
                response.GetResponseStream();

            Bitmap bmp = new Bitmap(responseStream);

            responseStream.Dispose();
            return bmp;
        }

        public Bitmap ResizeImage(Image image, int width, int height, bool keepProportion)
        {
            Bitmap result = null;
            double ratio = 1;
            try
            {
                if (keepProportion)
                {
                    double ratioX = (double)width / image.Width;
                    double ratioY = (double)height / image.Height;
                    ratio = ratioX < ratioY ? ratioX : ratioY;

                    width = (int)(image.Width * ratio);
                    height = (int)(image.Height * ratio);
                }

                result = new Bitmap(width, height);
                using (Graphics graphics = Graphics.FromImage(result))
                {
                    graphics.DrawImage(image, 0, 0, width, height);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return result;
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            RefreshList();
            LoadListView();
        }
    }
}
