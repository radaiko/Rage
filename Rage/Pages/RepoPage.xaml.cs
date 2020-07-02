using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace Rage.Pages
{
    public class RepoPage : UserControl
    {
        public RepoPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            ActivateCommitButtionWhenSummaryEdit();

        }

        private void ActivateCommitButtionWhenSummaryEdit(){
            var commitSummary = this.FindControl<TextBox>("CommitSummary");
            var summary = commitSummary.GetObservable(TextBox.TextProperty);
            summary.Subscribe(value =>
            {
                if (value == null || value == "")
                {
                    this.FindControl<Button>("Commit").Classes.Remove("active");
                } else 
                {
                    this.FindControl<Button>("Commit").Classes.Add("active");
                }
            });

        }

    }
}