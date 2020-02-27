using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using CloudPadApi;
using CloudPadApi.Http;
using CloudPadApi.DbModels;
using Newtonsoft.Json;
using Android.Support.V7.App;

namespace CloudPad.Activities
{
    [Activity(Label = "LoginActivity",MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        
        ProgressBar progressBar;
        TextInputLayout emailText;
        TextInputLayout passwordText;
        CoordinatorLayout rootView;
        private Button mBtnSignUp;
        TextView registerBtn;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.authorization_activity);
            ConnectControl();
     
        }


        void ConnectControl()
        {
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);

            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnSignUp.Click += MBtnSignUp_Click;

        }

        private void MBtnSignUp_Click(object sender, EventArgs e)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            View mView = LayoutInflater.Inflate(Resource.Layout.SignUpDialog, null);

            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);

            alertDialogBuilder.SetView(mView);

            var email = mView.FindViewById<EditText>(Resource.Id.editTextEmail);
            var password = mView.FindViewById<EditText>(Resource.Id.editTextPassword);

            alertDialogBuilder.SetCancelable(true).SetPositiveButton("Login", async delegate
            {
                progressBar.Visibility = ViewStates.Visible;
                CloudApi cloudApi = new CloudApi();


                var user_model = new User_Model { Email = email.Text, Password = password.Text };
                var result = await cloudApi.GetTokenAsync(user_model);
                if(result)
                {
                    Intent MainActivity = new Intent(this, typeof(MainActivity));
                    MainActivity.PutExtra("CloudApi", JsonConvert.SerializeObject(cloudApi));
                    this.StartActivity(MainActivity);
                    this.Finish();
                }
                else
                {
                    Toast.MakeText(this, cloudApi.Server_Msg, ToastLength.Long).Show();

                }
                progressBar.Visibility = ViewStates.Gone;
            });

            Android.Support.V7.App.AlertDialog alertDialog = alertDialogBuilder.Create();
            alertDialog.Show();

        }
    }
}