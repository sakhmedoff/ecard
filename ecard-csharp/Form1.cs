using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eCard
{
    public partial class Form1 : Form
    {
        // DEFAULT CONST VALUES
        Color DEFAULT_VIGNETTE_COLOR = Color.Black;
        Color DEFAULT_TEXT_COLOR = Color.Black;
        int DEFAULT_BLUR_VALUE = 5; // px
        int DEFALUT_ROUNDER_CORNER_VALUE = 20; // px

        //IMAGE VARIABLES
        string targetpicture;
        ImageProcessor.ImageFactory targerImg;

        ResourceManager rm;



        public Form1()
        {
            InitializeComponent();
            rm = new ResourceManager("eCard", System.Reflection.Assembly.GetExecutingAssembly());
        }

        private void appToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // OpenFile Dialog Method -> through button1 (Load Image BTN)
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            DialogResult result = openFileDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                targetpicture = openFileDialog1.FileName;
                try
                {
                       previewbox.Load(targetpicture);
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            
        }

        // !MAIN! Generating Method -> through button2 ( Generate BTN)
        private void button2_Click(object sender, EventArgs e)
        {
            targerImg = new ImageProcessor.ImageFactory();
            try {
                if (targetpicture != null)
                {
                    targerImg.Load(targetpicture);
                } else
                {
                    targerImg.Load(previewbox.Image);
                }
                
            } catch (Exception ex) {
                MessageBox.Show("You have not loaded image! Please click 'Load Image' button", "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            string path = System.IO.Path.GetRandomFileName().ToString() + ".png";
            

            try
            {
                if (comboBox1.SelectedIndex >= 0)
                {
                    switch (comboBox1.SelectedIndex)
                    {
                        case 0:
                            targerImg.Filter(ImageProcessor.Imaging.Filters.Photo.MatrixFilters.BlackWhite);
                            Debug.WriteLine(comboBox1.SelectedText);
                            break;
                        case 1:
                            targerImg.Filter(ImageProcessor.Imaging.Filters.Photo.MatrixFilters.Comic);
                            break;
                        case 2:
                            targerImg.Filter(ImageProcessor.Imaging.Filters.Photo.MatrixFilters.Gotham);
                            break;
                        case 3:
                            targerImg.Filter(ImageProcessor.Imaging.Filters.Photo.MatrixFilters.GreyScale);
                            break;
                        case 4:
                            targerImg.Filter(ImageProcessor.Imaging.Filters.Photo.MatrixFilters.HiSatch);
                            break;
                        case 5:
                            targerImg.Filter(ImageProcessor.Imaging.Filters.Photo.MatrixFilters.Invert);
                            break;
                        case 6:
                            targerImg.Filter(ImageProcessor.Imaging.Filters.Photo.MatrixFilters.Lomograph);
                            break;
                        case 7:
                            targerImg.Filter(ImageProcessor.Imaging.Filters.Photo.MatrixFilters.LoSatch);
                            break;
                        case 8:
                            targerImg.Filter(ImageProcessor.Imaging.Filters.Photo.MatrixFilters.Polaroid);
                            break;
                        case 9:
                            targerImg.Filter(ImageProcessor.Imaging.Filters.Photo.MatrixFilters.Sepia);
                            break;
                    }
                }
                if (blur_cb.Checked == true)
                {
                    targerImg.GaussianBlur(10);
                }
                if (vignette_cb.Checked == true)
                {
                    if (colorDialog_Vignette.Color == null)
                    {
                        targerImg.Vignette(DEFAULT_VIGNETTE_COLOR);
                    }
                    else
                    {
                        targerImg.Vignette(colorDialog_Vignette.Color);
                    }
                }
                if (round_cb.Checked == true)
                {
                    targerImg.RoundedCorners(25);
                }
                if (richTextBox1.Text != "")
                {
                    using (ImageProcessor.Imaging.TextLayer Text = new ImageProcessor.Imaging.TextLayer())
                    {
                        Text.Text = richTextBox1.Text.ToString();
                        if (checkBox1.Checked == true) {
                            Text.DropShadow = true;
                        } else
                        {
                            Text.DropShadow = false;
                        }
                        Text.FontSize = Convert.ToInt32(fontDialog1.Font.Size);
                        Text.FontColor = colorDialog1.Color;
                        Text.FontFamily = fontDialog1.Font.FontFamily;
                        

                        targerImg.Watermark(Text);
                    }
                }

                targerImg.Save(System.IO.Path.GetTempPath().ToString() + path);
                
                pictureBox1.Load(System.IO.Path.GetTempPath().ToString() + path);
            }
            catch(Exception ex)
            {

            }

           
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            
            
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string a = saveFileDialog1.FileName;
            targerImg.Save(a + ".png");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            previewbox.Image = null;
            richTextBox1.Text = "";
            round_cb.Checked = false;
            vignette_cb.Checked = false;
            blur_cb.Checked = false;
            pictureBox1.Image = null;
            colorDialog1.Color = DEFAULT_TEXT_COLOR;
            colorDialog_Vignette.Color = DEFAULT_VIGNETTE_COLOR;
            try {
                targerImg.Dispose();
            } catch (Exception ex) { }
            finally { }
            targetpicture = null;
            trackBar1.Value = DEFAULT_BLUR_VALUE;
            trackBar2.Value = DEFALUT_ROUNDER_CORNER_VALUE;
            

        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            colorDialog_Vignette.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            blur_value.Text = trackBar1.Value.ToString() + " px";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            round_value.Text = trackBar2.Value.ToString() + " px";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            comboBox2.Show();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    previewbox.Image = Properties.Resources.Shrimpy;
                    break;
                case 1:
                    previewbox.Image = Properties.Resources.Influenza;
                    break;
                case 2:
                    previewbox.Image = Properties.Resources.Bourbon;
                    break;
                case 3:
                    previewbox.Image = Properties.Resources.Calm_Darya;
                    break;
                case 4:
                    previewbox.Image = Properties.Resources.Day_Tripper;
                    break;
                case 5:
                    previewbox.Image = Properties.Resources.Kyoto;
                    break;
                case 6:
                    previewbox.Image = Properties.Resources.Titanium;
                    break;
                case 7:
                    previewbox.Image = Properties.Resources.Miaka;
                    break;
                case 8:
                    previewbox.Image = Properties.Resources.Mantle;
                    break;
            }
        }
    }
}
