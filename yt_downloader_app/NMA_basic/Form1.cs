using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Videos;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Common;
using YoutubeExplode.Channels;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.ClosedCaptions;
using System.Threading;

namespace NMA_basic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string yol = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox3.Hide();
            axWindowsMediaPlayer1.settings.autoStart = true;
            listBox2.Hide();
            Calma_Listelerini_Goster();
        }
        void Calma_Listelerini_Goster()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            string yeni_yol = yol.Substring(6) + @"\Playlists\";
            //
            try
            {
                string[] directories = Directory.GetDirectories(yeni_yol);

                foreach (string listeler in directories)
                {
                    string ismi = Path.GetFileName(listeler);
                    comboBox1.Items.Add(ismi);
                    comboBox2.Items.Add(ismi);
                    comboBox3.Items.Add(listeler);
                }
            }
            catch
            {
               // MessageBox.Show("Klasör Bulunmadı!");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            comboBox3.SelectedIndex = comboBox1.SelectedIndex;
            DirectoryInfo dirInfoFile = new DirectoryInfo(comboBox3.SelectedItem.ToString());
            FileInfo[] files = dirInfoFile.GetFiles();
            foreach (FileInfo fi in files)
            {
                string kisa = fi.Name.Substring(0, fi.Name.Length - 4);
                listBox1.Items.Add(kisa);
                listBox2.Items.Add(fi.FullName);                       
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //  MessageBox.Show("Şarkıların Oynatılması için bunu kapatma!");
                listBox2.SelectedIndex = listBox1.SelectedIndex;
                label4.Text = listBox1.SelectedItem.ToString();


                axWindowsMediaPlayer1.URL = listBox2.SelectedItem.ToString();

            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string yeni_yol = yol.Substring(6) + @"\Playlists\";
            Directory.CreateDirectory(yeni_yol + textBox1.Text);
            textBox1.Clear();
            MessageBox.Show("Playlist Created!");
            Calma_Listelerini_Goster();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.SelectedIndex = comboBox2.SelectedIndex;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string yeni_yol = yol.Substring(6) + @"\Playlists\" + comboBox2.Text + @"\";
            string videoUrl = textBox2.Text;

            var youtube = new YoutubeClient();
            if(comboBox2.SelectedItem != null)
            {
                try
                {
                    var video = await youtube.Videos.GetAsync(videoUrl);
                    var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                    var streamInfo = streamInfoSet.GetMuxedStreams().TryGetWithHighestVideoQuality();

                    if (streamInfo != null)
                    {
                        await youtube.Videos.Streams.DownloadAsync(streamInfo, yeni_yol + textBox3.Text + ".mp4");
                        label3.Text = "Music has been added to your playlist!";
                        Thread.Sleep(3000);
                        label3.Text = "";
                    }
                }
                catch (VideoUnavailableException)
                {
                    MessageBox.Show("Error");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select a playlist!");
            }
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8)
            {
                try
                {
                    listBox1.SelectedIndex += 1;

                }
                catch
                {
                    listBox1.SelectedIndex = 0;
                }
               
            }
        }

        void oynat()
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
    
}
