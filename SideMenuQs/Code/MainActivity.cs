using Android.App;
using Android.OS;
using Yalantis.Com.Sidemenu.Interfaces;
using Yalantis.Com.Sidemenu.Animation;
using Yalantis.Com.Sidemenu.Model;
using SideMenuQs.Code;
using System.Collections.Generic;
using Android.Support.V4.Widget;
using Android.Graphics;
using Android.Support.V7.AppCompat;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using Yalantis.Com.Sidemenu.Util;
using IO.Codetail.Animation;
using Android.Views;
using System;
using Android.Views.Animations;
using Android.Graphics.Drawables;
using Android.Content.Res;

namespace SideMenuQs
{
    [Activity(Label = "SideMenuQs", MainLauncher = true, Icon = "@drawable/icon",Theme = "@style/AppTheme")]
    public class MainActivity : ActionBarActivity, Yalantis.Com.Sidemenu.Util.ViewAnimator.IViewAnimatorListener
    {
        private List<SlideMenuItem> listMenuItems = new List<SlideMenuItem>();
        private Yalantis.Com.Sidemenu.Util.ViewAnimator viewAnimator;
        private DrawerLayout drawerLayout;
        private ActionBarDrawerToggle drawerToggle;
        private ContentFragment contentFragment;
        private int res = Resource.Drawable.content_music;
        private LinearLayout linearLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            contentFragment = ContentFragment.NewInstance(Resource.Drawable.content_music);
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, contentFragment)
                .Commit();

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawerLayout.SetScrimColor(Color.Transparent);
            linearLayout = FindViewById<LinearLayout>(Resource.Id.left_drawer);
            linearLayout.Click += (s, e) =>
            {
                drawerLayout.CloseDrawers();
            };

            SetActionBar();
            CreateMenuList();

            viewAnimator = new Yalantis.Com.Sidemenu.Util.ViewAnimator(this, listMenuItems, contentFragment, drawerLayout, this);

        }

        public void CreateMenuList()
        {
            SlideMenuItem menuItem0 = new SlideMenuItem("CLOSE", Resource.Drawable.icn_close);
            listMenuItems.Add(menuItem0);
            SlideMenuItem menuItem = new SlideMenuItem("BUILDING", Resource.Drawable.icn_1);
            listMenuItems.Add(menuItem);
            SlideMenuItem menuItem2 = new SlideMenuItem("BOOK", Resource.Drawable.icn_2);
            listMenuItems.Add(menuItem2);
            SlideMenuItem menuItem3 = new SlideMenuItem("PAINT", Resource.Drawable.icn_3);
            listMenuItems.Add(menuItem3);
            SlideMenuItem menuItem4 = new SlideMenuItem("CASE", Resource.Drawable.icn_4);
            listMenuItems.Add(menuItem4);
            SlideMenuItem menuItem5 = new SlideMenuItem("SHOP", Resource.Drawable.icn_5);
            listMenuItems.Add(menuItem5);
            SlideMenuItem menuItem6 = new SlideMenuItem("PARTY", Resource.Drawable.icn_6);
            listMenuItems.Add(menuItem6);
            SlideMenuItem menuItem7 = new SlideMenuItem("MOVIE", Resource.Drawable.icn_7);
            listMenuItems.Add(menuItem7);
        }

        public void SetActionBar()
        {
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetHomeButtonEnabled(true);
            drawerToggle = new ActionBarDrawerToggle0(
                this,
                drawerLayout,
                toolbar,
                Resource.String.drawer_open,
                Resource.String.drawer_close)
            {
                DrawerClosed = (View view) =>
                {
                    linearLayout.RemoveAllViews();
                    linearLayout.Invalidate();
                },
                DrawerSlide = (View drawerView, float slideOffset) =>
                {
                    if (slideOffset > 0.6 && linearLayout.ChildCount == 0) viewAnimator.ShowMenuContent();
                },
                DrawerOpened = (View drawerView) =>
                {

                }
            };

            drawerLayout.SetDrawerListener(drawerToggle);
        }

        public override void OnPostCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnPostCreate(savedInstanceState, persistentState);
            drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            drawerToggle.OnConfigurationChanged(newConfig);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (drawerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }

            switch (item.ItemId)
            {
                case Resource.Id.action_settings:
                    return true;
                default: return base.OnOptionsItemSelected(item);
            }
        }

        public IScreenShotable ReplaceFragment(IScreenShotable screenShotable, int topPosition)
        {
            this.res = this.res == Resource.Drawable.content_music ? Resource.Drawable.content_films : Resource.Drawable.content_music;
            View view = FindViewById(Resource.Id.content_frame);
            int finalRadius = Math.Max(view.Width, view.Height);
            SupportAnimator animator = IO.Codetail.Animation.ViewAnimationUtils.CreateCircularReveal(view, 0, topPosition, 0, finalRadius);
            animator.SetInterpolator(new AccelerateInterpolator());
            animator.SetDuration(Yalantis.Com.Sidemenu.Util.ViewAnimator.CircularRevealAnimationDuration);

            FindViewById(Resource.Id.content_overlay).SetBackgroundDrawable(new BitmapDrawable(Resources, screenShotable.Bitmap));
            animator.Start();
            ContentFragment contentFragment = ContentFragment.NewInstance(this.res);
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, contentFragment).Commit();
            return contentFragment;
        }

        public void AddViewToContainer(View p0)
        {
            linearLayout.AddView(p0);
        }

        public void DisableHomeButton()
        {
            SupportActionBar.SetHomeButtonEnabled(false);
        }

        public void EnableHomeButton()
        {
            SupportActionBar.SetHomeButtonEnabled(true);
            drawerLayout.CloseDrawers();
        }

        public IScreenShotable OnSwitch(IResourceble p0, IScreenShotable p1, int p2)
        {
            switch (p0.Name)
            {
                case ContentFragment.CLOSE:
                    return p1;
                default:
                    return ReplaceFragment(p1, p2);
            }
        }

    }
    public class ActionBarDrawerToggle0 : ActionBarDrawerToggle
    {
        public Action<View> DrawerClosed { set; get; }
        public Action<View, float> DrawerSlide { get; set; }
        public Action<View> DrawerOpened { get; set; }

        public ActionBarDrawerToggle0(Activity activity,
            DrawerLayout drawerLayout,
            Android.Support.V7.Widget.Toolbar toolbar,
            int openDrawerContentDescRes,
            int closeDrawerContentDescRes)
            : base(activity,
                drawerLayout,
                toolbar,
                openDrawerContentDescRes,
                closeDrawerContentDescRes)
        {

        }

        public override void OnDrawerClosed(View drawerView)
        {
            base.OnDrawerClosed(drawerView);
            DrawerClosed?.Invoke(drawerView);
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            base.OnDrawerSlide(drawerView, slideOffset);
            DrawerSlide?.Invoke(drawerView, slideOffset);
        }

        public override void OnDrawerOpened(View drawerView)
        {
            base.OnDrawerOpened(drawerView);
            DrawerOpened?.Invoke(drawerView);
        }
    }

}

