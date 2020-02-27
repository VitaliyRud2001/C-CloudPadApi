using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using CloudPad;
using CloudPadApi.Http;
using CloudPadApi.DbModels;
namespace CloudPad.Activities
{
    [Activity(Label = "RegistrationActivity",MainLauncher =false)]
    public class RegistrationActivity : AppCompatActivity
    {
        ProgressBar progressBar;
        TextInputLayout emailText;
        TextInputLayout passwordText;
        CoordinatorLayout rootView;
        Button registerBtn;
        TextView loginBtn;
        string Email, Password;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.registration_activity);
            ConnectControl();

            // Create your application here
        }

        void ConnectControl()
        {
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            progressBar.Visibility = ViewStates.Gone;

            loginBtn = FindViewById<TextView>(Resource.Id.loginTextView);
            loginBtn.Click += LoginBtn_Click;

            emailText = FindViewById<TextInputLayout>(Resource.Id.emailText);
            passwordText = FindViewById<TextInputLayout>(Resource.Id.passwordText);
            rootView = FindViewById<CoordinatorLayout>(Resource.Id.rootView);
            registerBtn = FindViewById<Button>(Resource.Id.registerBtn);

            registerBtn.Click += RegisterBtn_Click;

        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            Intent LoginActivity = new Intent(this, typeof(LoginActivity));
            StartActivity(LoginActivity);
        }

        private async void RegisterBtn_Click(object sender, EventArgs e)
        {
            registerBtn.Visibility = ViewStates.Gone;
            progressBar.Visibility = ViewStates.Visible;

            CloudApi cloudApi = new CloudApi();
            User_Model user = new User_Model { Email = emailText.EditText.Text, Password = passwordText.EditText.Text };

            var result = await cloudApi.RegisterUserAsync(user);
            if(result)
            {
                Snackbar.Make(rootView, "Registered successfuly", Snackbar.LengthLong).Show();
            }
            else
            {
                Snackbar.Make(rootView, cloudApi.Server_Msg, Snackbar.LengthLong).Show();
            }

            registerBtn.Visibility = ViewStates.Visible;
            progressBar.Visibility = ViewStates.Gone;

        }
    }
}