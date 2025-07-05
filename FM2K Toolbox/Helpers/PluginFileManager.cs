using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class PluginFileManager
{
    private readonly FlowLayoutPanel panelContainer;
    private readonly Button removeButton;
    private readonly Form parentForm;
    private readonly string iniFilePath;
    private SimpleFileStore fileStore;

    public PluginFileManager(Form form, FlowLayoutPanel panel, Button removeBtn)
    {
        parentForm = form;
        panelContainer = panel;
        removeButton = removeBtn;

        fileStore = new SimpleFileStore(Path.Combine(Application.StartupPath, "plugins.ini"));

        LoadFromIni();
    }

    // Add file to label and .ini file
    public void AddFile(string filePath, bool saveToIni = true)
    {
        if (IsFileAlreadyAdded(filePath))
            return;

        Panel panel = new Panel();
        panel.Width = panelContainer.ClientSize.Width - 25;
        panel.Height = 30;
        panel.Margin = new Padding(3);

        CheckBox checkBox = new CheckBox
        {
            Width = 20,
            Height = 20,
            Location = new Point(5, 5)
        };

        Label lbl = new Label
        {
            Text = Path.GetFileName(filePath),
            Tag = filePath,
            AutoSize = false,
            Width = panel.Width - 30,
            Height = 25,
            Location = new Point(30, 5)
        };

        panel.Controls.Add(checkBox);
        panel.Controls.Add(lbl);

        panelContainer.Controls.Add(panel);
        UpdateRemoveButtonState();

        if (saveToIni)
            fileStore.AppendLine(filePath);
    }

    // Remove files label and from .ini file
    public void RemoveFiles()
    {
        var panelsToRemove = new List<Control>();
        var removedPaths = new List<string>();

        foreach (Control ctrl in panelContainer.Controls)
        {
            if (ctrl is Panel panel)
            {
                foreach (Control child in panel.Controls)
                {
                    if (child is CheckBox checkBox && checkBox.Checked)
                    {
                        Label label = null;
                        foreach (Control control in panel.Controls)
                        {
                            if (control is Label)
                            {
                                label = (Label)control;
                                break;
                            }
                        }
                        if (label != null && label.Tag is string filePath)
                        {
                            removedPaths.Add(filePath);
                        }

                        panelsToRemove.Add(panel);
                        break;
                    }
                }
            }
        }

        if (panelsToRemove.Count == 0)
        {
            MessageBox.Show(parentForm,
                "Please select at least one file to remove.",
                "No Selection",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show(parentForm,
            $"Are you sure you want to remove {panelsToRemove.Count} file(s)?",
            "Confirm Removal",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            foreach (var panel in panelsToRemove)
            {
                panelContainer.Controls.Remove(panel);
                panel.Dispose();
            }

            fileStore.RemoveLines(removedPaths);
            UpdateRemoveButtonState();
        }
    }

    // Update Remove Button State
    private void UpdateRemoveButtonState()
    {
        removeButton.Enabled = panelContainer.Controls.Count > 0;
    }

    // Load saved files path in the .ini file
    private void LoadFromIni()
    {
        List<string> lines = fileStore.LoadLines();

        foreach (string file in lines)
        {
            if (File.Exists(file))
            {
                AddFile(file, saveToIni: false);
            }
        }
    }

    // validate if file labels already exist
    private bool IsFileAlreadyAdded(string filePath)
    {
        foreach (Control ctrl in panelContainer.Controls)
        {
            if (ctrl is Panel panel)
            {
                Label label = null;
                foreach (Control child in panel.Controls)
                {
                    if (child is Label)
                    {
                        label = (Label)child;
                        break;
                    }
                }
                if (label != null && label.Tag is string path && path == filePath)
                    return true;
            }
        }
        return false;
    }
}
