using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structurizr.Client;

namespace Structurizr.Examples
{
    class BookingSample
    {
        public static void Main(string[] args)
        {
            Workspace bookingWorkspace = new Workspace("Co-Working Booking System", "This is a system to manage a Co-working Business");
            Model model = bookingWorkspace.Model;
            SoftwareSystem bookingSystem = model.AddSoftwareSystem(Location.Internal, "Booking System",
                "Allow to Manage a Co-Working Busines, from the booking to the payment");
            bookingSystem.AddTags("BookingSystem");

            Person manager = model.AddPerson(Location.Internal, "Manager", "User which Manage the Cor-Working Business");
            Person subscriber = model.AddPerson(Location.External, "Register Subscriber", "User which has a subscription in the Co-Working System");
            Person internalUser = model.AddPerson(Location.Internal, "Internal User",
                "User who perform all the maintenance duties and register changes in the bookings");

            SoftwareSystem mailSystem = model.AddSoftwareSystem(Location.External, "Mail System",
                "SMTP service which deliver all the email notifications");
            SoftwareSystem identityApp = model.AddSoftwareSystem(Location.External, "Identity Server",
                "Allow Authorization/Authentication of the users");

            bookingSystem.Uses(identityApp, "Authenticates and Authorize the user ", "HTTP", InteractionStyle.Asynchronous);
            bookingSystem.Uses(mailSystem, "Send notifications", "SSMTP/HTTPS", InteractionStyle.Asynchronous);

            mailSystem.Delivers(subscriber, "Email notification", "Email message", InteractionStyle.Asynchronous);
            manager.Uses(bookingSystem, "Approves new Subscriptions, Get Manage Reports", "http");
            subscriber.Uses(bookingSystem, "Manage his bookings, get account status reports");
            internalUser.Uses(bookingSystem,
                "Register Payments, Booking Expenses, Correspondence, Call and another services for the subscribers");

            ViewSet viewSet = bookingWorkspace.Views;
            SystemContextView contextView = viewSet.CreateSystemContextView(bookingSystem, "booking",
                "Allow to manage Co-working business");

            contextView.PaperSize = PaperSize.A3_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Element) { Color = "#ffffff", FontSize = 34 });
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Width = 650, Height = 400, Background = "#ff3333", Shape = Shape.Box });
            styles.Add(new ElementStyle(Tags.Person) { Width = 550, Background = "#3385ff", Shape = Shape.Person });
            styles.Add(new ElementStyle("BookingSystem") { Background = "#000099" });

            styles.Add(new RelationshipStyle(Tags.Relationship) { Thickness = 4, Dashed = false, FontSize = 32, Width = 400 });
            styles.Add(new RelationshipStyle(Tags.Synchronous) { Dashed = false });
            styles.Add(new RelationshipStyle(Tags.Asynchronous) { Dashed = true });
            styles.Add(new RelationshipStyle("Alert") { Color = "#ff0000" });

            StructurizrClient structurizrClient = new StructurizrClient("c61aedc0-5c14-4483-8a53-3da06132cbec", "3f7c40c8-e92a-41c1-9f93-773ff53eef02");
            structurizrClient.PutWorkspace(16191, bookingWorkspace);

        }
    }
}
