using NeuronDotNet.Core;
using NeuronDotNet.Core.Backpropagation;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private static BackpropagationNetwork neuralNetwork;
        private SigmoidLayer hiddenTier;
        private SigmoidLayer outputTier;
        private LinearLayer inputTier;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_OnLoad(object sender, EventArgs e)
        {
            inputTier = new LinearLayer(35);
            hiddenTier = new SigmoidLayer(3);
            outputTier = new SigmoidLayer(5);
            _ = new BackpropagationConnector(inputTier, hiddenTier);
            _ = new BackpropagationConnector(hiddenTier, outputTier);
            neuralNetwork = new BackpropagationNetwork(inputTier, outputTier);
            neuralNetwork.Initialize();
        }
        //kutucuklara tıklandığında gerçekleşen action
        private void btnMatris_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button.BackColor == Color.Green)
            {
                button.BackColor = SystemColors.Control;
                button.Text = "";
                return;
            }

            button.BackColor = Color.Green;
            button.Text = "X";
        }
        // reset butonu
        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (Button item in pixelContainer.Controls)
            {
                item.BackColor = SystemColors.Control;
            }
        }
        // train butonu
        private void btnTrain_Click(object sender, EventArgs e)
        {
            TrainingSet trainingSet = new TrainingSet(35, 5);
            trainingSet.Add(new TrainingSample(Dataset.Letters.A, new double[5] { 1, 0, 0, 0, 0 }));
            trainingSet.Add(new TrainingSample(Dataset.Letters.B, new double[5] { 0, 1, 0, 0, 0 }));
            trainingSet.Add(new TrainingSample(Dataset.Letters.C, new double[5] { 0, 0, 1, 0, 0 }));
            trainingSet.Add(new TrainingSample(Dataset.Letters.D, new double[5] { 0, 0, 0, 1, 0 }));
            trainingSet.Add(new TrainingSample(Dataset.Letters.E, new double[5] { 0, 0, 0, 0, 1 }));
            neuralNetwork.SetLearningRate(Convert.ToDouble(0.3));
            neuralNetwork.Learn(trainingSet, Convert.ToInt32(5000));
            btnTrain.Enabled = false;
            btnGetResults.Enabled = true;
        }
        // get results butonu
        private void btnGetResult_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                listBox1.Items.Clear();
                txtMatrix.Clear();
            }
            // 7x5 matris
            double[][] dizi = new double[7][];
            {
                dizi[0] = new double[] { 0, 0, 0, 0, 0 };
                dizi[1] = new double[] { 0, 0, 0, 0, 0 };
                dizi[2] = new double[] { 0, 0, 0, 0, 0 };
                dizi[3] = new double[] { 0, 0, 0, 0, 0 };
                dizi[4] = new double[] { 0, 0, 0, 0, 0 };
                dizi[5] = new double[] { 0, 0, 0, 0, 0 };
                dizi[6] = new double[] { 0, 0, 0, 0, 0 };
            };
            // kutucuklara göre 0 ve 1'lerden matris oluşturma
            foreach (Control buttons in pixelContainer.Controls)
            {
                Button button = buttons as Button;

                int i = Convert.ToInt32(button.Tag.ToString());
                if (buttons.Text == "X" && buttons.BackColor == Color.Green)
                {
                    if (i <= 4)
                    {
                        dizi[0][i] = 1;
                    }
                    else if (i <= 9)
                    {
                        dizi[1][i % 5] = 1;
                    }
                    else if (i <= 14)
                    {
                        dizi[2][i % 5] = 1;
                    }
                    else if (i <= 19)
                    {
                        dizi[3][i % 5] = 1;
                    }
                    else if (i <= 24)
                    {
                        dizi[4][i % 5] = 1;
                    }
                    else if (i <= 29)
                    {
                        dizi[5][i % 5] = 1;
                    }
                    else if (i <= 34)
                    {
                        dizi[6][i % 5] = 1;
                    }
                }
                i++;
            }
            double[] outputResult = new double[35];
            // 0 ve 1'lerden oluşan matris sonucunu diziye yazdırma
            int b = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    outputResult[b] = dizi[i][j];
                    b++;
                }
            }

            double[] input = outputResult;
            //yapay sinir ağlarına göre çıktı alma 
            double[] output = neuralNetwork.Run(input);

            // sonuçları listbox'a yazdırma
            for (int j = 1; j < output.Length + 1; j++)
            {
                switch (j)
                {
                    case 1:
                        listBox1.Items.Add($"A : {output[j - 1]:%.####} \n");
                        break;
                    case 2:
                        listBox1.Items.Add($"B : {output[j - 1]:%.####} \n");
                        break;
                    case 3:
                        listBox1.Items.Add($"C : {output[j - 1]:%.####} \n");
                        break;
                    case 4:
                        listBox1.Items.Add($"D : {output[j - 1]:%.####} \n");
                        break;
                    case 5:
                        listBox1.Items.Add($"E : {output[j - 1]:%.####} \n");
                        listBox1.Items.Add("MSE: " + neuralNetwork.MeanSquaredError.ToString());
                        break;
                    default:
                        Console.WriteLine("Gecersiz.");
                        break;
                }
            }

            // 0 ve 1'lerden oluşturulan matrisi ekrana yazdırma
            for (int k = 0; k < input.Length; k++)
            {
                txtMatrix.Text += (input[k].ToString() + "    ");
                if ((k + 1) % 5 == 0)
                {
                    txtMatrix.Text += "\n";
                }
            }
            txtMatrix.SelectAll();
            txtMatrix.SelectionAlignment = HorizontalAlignment.Center;
            txtMatrix.DeselectAll();
        }
    }
}
