using UnityEngine;
using System.ComponentModel;
using System.Net.Mail;
using TMPro;
using System.Text.RegularExpressions;

public class EmailSender : MonoBehaviour
{
    public string senderEmail = "snaptive.studio@gmail.com";
    public string senderPassword = "fhqpqxhiacysrcoy";
    public string SenderName = "Snaptive Studio";

    [Space]
    public string subject;
    [TextArea]
    public string body;

    private bool IsValidEmail(string email)
    {
        // Ekspresi reguler untuk memeriksa email
        string emailPattern = @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$";
        Regex regex = new Regex(emailPattern);

        return regex.IsMatch(email);
    }

    public void SendEmail(TMP_InputField _inputField)
    {
        string recipientEmail = _inputField.text;

        if (string.IsNullOrEmpty(recipientEmail) && !IsValidEmail(recipientEmail))
        {
            Debug.Log("Invalid email address!");
            return;
        }

        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
        client.Credentials = new System.Net.NetworkCredential(
            senderEmail,
            senderPassword);
        client.EnableSsl = true;

        // Specify the email sender.
        // Create a mailing address that includes a UTF8 character
        // in the display name.
        MailAddress from = new MailAddress(
            senderEmail,
            SenderName,
            System.Text.Encoding.UTF8);
        // Set destinations for the email message.
        MailAddress to = new MailAddress(recipientEmail);

        // Specify the message content.
        MailMessage message = new MailMessage(from, to);
        message.Subject = subject;
        message.SubjectEncoding = System.Text.Encoding.UTF8;
        message.Body = body;
        message.BodyEncoding = System.Text.Encoding.UTF8;
        string attachmentPath = $"{PrintHandler.Instance.photoLocation}";
        Attachment attachment = new Attachment(attachmentPath);
        message.Attachments.Add(attachment);

        // Set the method that is called back when the send operation ends.
        client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        string userState = "test message1";
        client.SendAsync(message, userState);
    }

    private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        // Get the unique identifier for this asynchronous operation.
        string token = (string)e.UserState;

        if (e.Cancelled)
        {
            Debug.Log("Send canceled " + token);
        }
        if (e.Error != null)
        {
            Debug.Log("[ " + token + " ] " + " " + e.Error.ToString());
        }
        else
        {
            Debug.Log("Message sent.");
            this.gameObject.SetActive(false);
        }
    }
}
