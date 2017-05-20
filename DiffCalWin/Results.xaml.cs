using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;
using DifferenceEngine;
using System.Windows.Controls;
using System.Windows.Data;

namespace DiffCalWin
{
    /// <summary>
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class Results
    {
        public Results()
        {
            InitializeComponent();
        }

        private GridView CreateComparisonView(string sFileCategory)
        {
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Line",
                DisplayMemberBinding = new Binding("Line")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = $"Text ({sFileCategory})",
                DisplayMemberBinding = new Binding("Text")
            });
            return gridView;
        }

        public Results(DiffList_TextFile source, DiffList_TextFile destination, ArrayList DiffLines, double seconds)
        {
            InitializeComponent();
            this.Title = $"Results: {seconds:#0.00} secs.";

            var cnt = 1;

            foreach (DiffResultSpan diffResult in DiffLines)
            {
                switch (diffResult.Status)
                {
                    case DiffResultSpanStatus.DeleteSource:
                        for (var idx = 0; idx < diffResult.Length; idx++)
                        {
                            DgSource.Items.Add(new
                            {
                                Line = cnt,
                                Text = ((TextLine) source.GetByIndex(diffResult.SourceIndex + idx)).Line,
                                Type = "Red"
                            });

                            DgTarget.Items.Add(new
                            {
                                Line = cnt,
                                Text = "",
                                Type = "Gray"
                            });
                            cnt++;
                        }
                        break;
                    case DiffResultSpanStatus.NoChange:
                        for (var idx = 0; idx < diffResult.Length; idx++)
                        {
                            DgSource.Items.Add(new
                            {
                                Line = cnt,
                                Text = ((TextLine)source.GetByIndex(diffResult.SourceIndex + idx)).Line,
                                Type = "White"
                            });

                            DgTarget.Items.Add(new
                            {
                                Line = cnt,
                                Text = ((TextLine)destination.GetByIndex(diffResult.DestIndex + idx)).Line,
                                Type = "White"
                            });
                            cnt++;
                        }
                        break;
                    case DiffResultSpanStatus.AddDestination:
                        for (var idx = 0; idx < diffResult.Length; idx++)
                        {
                            DgSource.Items.Add(new
                            {
                                Line = cnt,
                                Text = "",
                                Type = "Gray"
                            });

                            DgTarget.Items.Add(new
                            {
                                Line = cnt,
                                Text = ((TextLine)destination.GetByIndex(diffResult.DestIndex + idx)).Line,
                                Type = "Green"
                            });
                            cnt++;
                        }
                        break;
                    case DiffResultSpanStatus.Replace:
                        for (var idx = 0; idx < diffResult.Length; idx++)
                        {
                            DgSource.Items.Add(new
                            {
                                Line = cnt,
                                Text = ((TextLine)source.GetByIndex(diffResult.SourceIndex + idx)).Line,
                                Type = "Red"
                            });

                            DgTarget.Items.Add(new
                            {
                                Line = cnt,
                                Text = ((TextLine)destination.GetByIndex(diffResult.DestIndex + idx)).Line,
                                Type = "Green"
                            });
                            cnt++;
                        }
                        break;
                }

            }
        }
    }
}
