﻿using InterfaceMeddelande;
using RSS_Demo.Data;
using RSS_Demo.Logik;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace RSS_Demo
{
    public partial class Form : System.Windows.Forms.Form, IMessage
    {
        private AsyncTimer timer;
        readonly private List<string> categoryList = CategoryRepo.LoadCategories();
        readonly private List<Podcast> podcastList = PodcastRepo.LoadPodcasts();
        private int interval = UpdateIntervalRepo.LoadUpdateInterval();
        private readonly MessageController ctrl;
        private readonly GreetingMsg greeting = new HolidayGreeting();
        static public int timerCounter { get; set; }

        public Form()
        {
            InitializeComponent();
            timerCounter = 0;
            StartTimer();
            MessageBox.Show(greeting.Greet());

            ctrl = new MessageController(this);
            podcastListview = PodcastHandler.updatePodcastListview(podcastListview);
            categoryListview = PodcastHandler.updateCategoryListview(categoryListview);
            categoryCombobox = PodcastHandler.updateComboBox(categoryCombobox); 
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
        }
        private void GroupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void Message()
        {
            MessageBox.Show("Kategorierna har sparats!");
        }

        private void ButtonURL_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validering.CheckIfURLIsValid(textBoxURL.Text).Length > 0)
                {
                    if (Validering.CheckIfComboboxIsEmpty(categoryCombobox))
                    {
                        PodcastHandler.addPodcast(textBoxURL.Text, categoryCombobox.Text, Int32.Parse(comboBoxUpdateInterval.Text.Substring(0, 1)));
                        podcastListview = PodcastHandler.updatePodcastListview(podcastListview);
                    }
                }
            }
            catch (WebException) { }
        }

        private void ButtonLaggTillKategori_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validering.CheckIfTextfieldsIsEmpty(categoryTextbox) && Validering.CheckIfTextfieldsHasANumber(categoryTextbox))
                {
                    if (Validering.CheckIfCategoryIsAvailable(categoryTextbox))
                    {
                        string kategoriInput = categoryTextbox.Text.Trim();
                        if (kategoriInput.Length != 0)
                        {
                            categoryListview.Items.Add(kategoriInput);
                            categoryCombobox.Items.Add(kategoriInput);
                            categoryList.Add(kategoriInput);
                        }
                        categoryTextbox.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Något gick fel vid inläsningen av kategorin.");
            }
        }

        private void ButtonTaBortKategori_Click(object sender, EventArgs e)
        {
            if(categoryListview.SelectedItems[0].Text.Length > 0)
            {
                if(Validering.validateSelectedCategory(categoryListview.SelectedItems[0].Text, "remove"))
                {
                        PodcastHandler.removeCategory(categoryListview.SelectedItems[0].Text);
                        categoryCombobox = PodcastHandler.updateComboBox(categoryCombobox);
                        categoryListview = PodcastHandler.updateCategoryListview(categoryListview);
                }
            }
            else
            {
                MessageBox.Show("Du måste välja en kategori för att ta bort den");
            }
        }

        

        private void ButtonSparaKategorier_Click(object sender, EventArgs e)
        {

            PodcastHandler.saveData("cat");


            ctrl.InterfaceMessage();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            // When the application is exiting, write the application data to the
            // user file and close it.

            PodcastHandler.saveData("all");
        }

        

        private void ListViewPodcasts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (podcastListview.SelectedItems.Count > 0)
            {
                var podcastData = PodcastHandler.getPodcast(podcastListview.SelectedItems.ToString());
                episodeListview.BeginUpdate();
                episodeListview = PodcastHandler.updateEpisodeListview(episodeListview, podcastListview);
                episodeListview.EndUpdate();
                episodeListview.Items[0].Selected = true;
            }
        }

        private void ListViewEpisode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (episodeListview.SelectedItems.Count > 0)
            {
                episodeDetailsTextBox.Text = PodcastHandler.updateEpisodeDetails(episodeListview, podcastListview);
            }
        }

        private void ListaKategorier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryListview.SelectedItems.Count > 0)
            {
                if (categoryListview.SelectedItems[0].Text == "Visa alla podcasts" && podcastListview.Items.Count > 0)
                {
                    podcastListview.BeginUpdate();
                    podcastListview = PodcastHandler.updatePodcastListview(podcastListview);
                    podcastListview.EndUpdate();
                    podcastListview.Items[0].Selected = true;
                }
                else if (podcastListview.Items.Count > 0 && PodcastHandler.lookupTrue(categoryListview.SelectedItems[0].Text, "podcast"))
                {
                    podcastListview.BeginUpdate();
                    podcastListview = PodcastHandler.updatePodcastListview(podcastListview, categoryListview.SelectedItems[0].Text);
                    podcastListview.EndUpdate();
                    podcastListview.Items[0].Selected = true;
                }
                else if (podcastListview.Items.Count == 0)
                {
                    MessageBox.Show("Du har inte lagt till några podcasts än");
                }
                else
                {
                    MessageBox.Show("Inga podcasts tillhör kategorin");
                }
            }
        }

        public void StartTimer()
        {
            if (interval > 0)
            {
                if (timer != null)
                {
                    timer.Dispose();
                }
                timer = new AsyncTimer(TimeSpan.FromSeconds(1), GetNewEpisode);

                timer.Start();
            }
        }

        public void GetNewEpisode()
        {
            //if (podcastListview.InvokeRequired)
            //{

            //    podcastListview.Invoke((MethodInvoker)GetNewEpisodes);
            //}
            //else
            //{

            //    podcastListview.BeginUpdate();
            //    podcastListview = PodcastHandler.updatePodcastListview(podcastListview, true);
            //    podcastListview.EndUpdate();


            //    if (podcastListview.Items.Count > 0)
            //    {
            //        podcastListview.Items[0].Selected = true;
            //    }
            //}
            timerCounter++;
            if(timerCounter == 10)
            {
                MessageBox.Show("hej");
                timerCounter = 0;
            }
        }

        private void ButtonSaveUpdateInterval_Click(object sender, EventArgs e)
        {
            switch (comboBoxUpdateInterval.Text)
            {
                case "10 min":
                    interval = 10;
                    break;

                case "5 min":
                    interval = 5;
                    break;

                case "1 min":
                    interval = 1;
                    break;
            }
            UpdateIntervalRepo.SaveUpdateInterval(interval);
            StartTimer();
        }

        

        private void ButtonTaBortPodcast_Click(object sender, EventArgs e)
        {
            if (podcastListview.SelectedItems.Count > 0)
            {
                PodcastHandler.removePodcast(podcastListview.SelectedIndices[0]);
                
                if(PodcastHandler.podcastListCount() > 0)
                {
                    podcastListview = PodcastHandler.updatePodcastListview(podcastListview);
                    podcastListview.Items[0].Selected = true;
                }
                else
                {
                    podcastListview.Items.Clear();
                    episodeListview.Items.Clear();
                    episodeDetailsTextBox.Clear();
                }
            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            if(podcastListview.SelectedItems.Count > 0)
            {
                if(Validering.CheckIfComboboxIsEmpty(comboBoxUpdateInterval) || Validering.CheckIfComboboxIsEmpty(categoryCombobox))
                {
                    PodcastHandler.updatePodcast(categoryCombobox.Text, Int32.Parse(comboBoxUpdateInterval.Text.Substring(0, 1)), podcastListview.SelectedItems[0].Text);
                    podcastListview = PodcastHandler.updatePodcastListview(podcastListview);
                }
            }
            else
            {
                MessageBox.Show("Vänligen välj en podcast att ändra");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            PodcastHandler.testmetod(podcastListview, episodeListview);
        }
    }
}