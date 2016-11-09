using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Structurizr.Client;

namespace Structurizr.Examples
{
    class Epiq11
    {
        static void Main(string[] args)
        {
            Workspace epiq11WorkSpace = new Workspace("Epiq11", "Model for Epiq11 Application");
            Model model = epiq11WorkSpace.Model;

            Person anonymousUser = model.AddPerson(Location.External, "Anonymous User","This user can navigate to all the public datapoints");
            Person internalUser = model.AddPerson(Location.Internal, "Internal User","Regular Business Epiq User which is Authenticated");
            Person dataUser = model.AddPerson(Location.Internal, "Data Maintenance User","Application User which can trigger data updates in ElasticSearch");

            SoftwareSystem epiq11 = model.AddSoftwareSystem(Location.Internal, "Epiq11",
                "Portal which provide access to the information for all Bankrupcy Data managed by Epiq Systems");

            SoftwareSystem notificationSystem = model.AddSoftwareSystem(Location.Internal,"Notification","Service which provides Email/SMS/MMS noticing services");
            SoftwareSystem authenticationService = model.AddSoftwareSystem(Location.Internal, "Authentication Service", "Provides services to authenticate internal/external users");
            SoftwareSystem debtorMatrix = model.AddSoftwareSystem(Location.External, "Debtor Matrix", "Provides access to the Debtor Matrix information");
            SoftwareSystem caseManagement = model.AddSoftwareSystem(Location.External, "Case Management", "Provides access to the Case Management information");

            anonymousUser.Uses(epiq11, "Can Navigate and search for public available information","Http");
            internalUser.Uses(epiq11, "Can Navigate and search through all the information stored in the System","Http");
            
            dataUser.Uses(epiq11, "Triggers a data update into", "Http",InteractionStyle.Asynchronous);

            epiq11.Uses(notificationSystem, "Send Notifications request", "JSON/HTTPS", InteractionStyle.Asynchronous);
            epiq11.Uses(authenticationService, "Authenticates internal/external users", "RPC/TCP", InteractionStyle.Synchronous);
            epiq11.Uses(debtorMatrix, "Gets information of Cases, Claims, Docket,Key Documents from", "RPC/TCP/Sql Server",
                InteractionStyle.Synchronous);

            epiq11.Uses(caseManagement, "Get information of Cases, Claims, Docket,Key Documents (New Information) from", "RPC/TCP/Sql Server",
                InteractionStyle.Synchronous);

            notificationSystem.Delivers(anonymousUser, "Send notification/alerts related with a docket update", "SMS/Email message",
                InteractionStyle.Asynchronous);
            notificationSystem.Delivers(internalUser, "Send notification/alerts related with a docket update", "SMS/Email message",
                InteractionStyle.Asynchronous);
            
            ViewSet viewSet = epiq11WorkSpace.Views;
            SystemContextView contextView = viewSet.CreateSystemContextView(epiq11, "Context","");
            contextView.PaperSize=PaperSize.A3_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            Styles styles = viewSet.Configuration.Styles;
            epiq11.AddTags("Chapter11 Portal");
            notificationSystem.AddTags("Twilio");
            authenticationService.AddTags("Authentication");

            styles.Add(new ElementStyle(Tags.Element) { Color = "#ffffff", FontSize = 34 });
            styles.Add(new ElementStyle("Chapter11 Portal") { Background ="#000099"});
            styles.Add(new ElementStyle("Twilio") { Background = "#ff3333" });
            styles.Add(new ElementStyle("Authentication") {Background = "#ff9900" });
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Width = 650, Height = 400, Background = "#004d1a", Shape = Shape.Box });
            styles.Add(new ElementStyle(Tags.Person) { Width = 550, Background = "#3385ff", Shape = Shape.Person });
            styles.Add(new ElementStyle(Tags.Container) { Width = 650, Height = 400, Background = "#00264d", Shape = Shape.RoundedBox });

            styles.Add(new ElementStyle("DataStore") { Width = 650, Height = 400, Background = "#00264d", Shape = Shape.Cylinder });
            styles.Add(new ElementStyle(Tags.Component) { Width = 550, Background = "#004080", Shape = Shape.RoundedBox });

            styles.Add(new RelationshipStyle(Tags.Relationship) { Thickness = 4, Dashed = false, FontSize = 32, Width = 400 });
            styles.Add(new RelationshipStyle(Tags.Synchronous) { Dashed = false });
            styles.Add(new RelationshipStyle(Tags.Asynchronous) { Dashed = true });
            styles.Add(new RelationshipStyle("Alert") { Color = "#ff0000" });

            // Container view

            Container webApp = epiq11.AddContainer("Web Application", "Allow to the user navigate, search over the Chapter11 Information", "Angular 1.45 /MVC 5.0 ,IIS 8.0");
            Container elasticSearch = epiq11.AddContainer("ElasticSearch DataStore", "Hold all the Repositories for Cases, Claims, Dockets, KeyDocuments", "ElasticSearch 1.7.5");
            elasticSearch.AddTags("DataStore");
            Container dataPopulation = epiq11.AddContainer("DataPopulation", "Get and Updates the information in ElasticSearch from the DM and CM Databases", "Selhosted WebApi / .Net 4.5.2");
            Container searchApi = epiq11.AddContainer("Search WebAPi", "Performs Queries over ElasticSearch Data Store", "WebApi 2.1 / .Net 4.5.2");
            Container realTimeNotifications = epiq11.AddContainer("Real Time Notifications Broker", "Send Notifications request for Email/SMS noticing", "Selfhost Windows Service, .Net 4.5.2");
            Container notificationRepository = epiq11.AddContainer("Notifications Repository", "Store Noticing Configurations", "SQL Server");
            notificationRepository.AddTags("DataStore");

            anonymousUser.Uses(webApp, "Can Navigate and search for public available information", "Http");
            internalUser.Uses(webApp, "Can Navigate and search through all the information stored in the System", "Http");
            dataUser.Uses(dataPopulation, "Triggers a data update into", "Http", InteractionStyle.Synchronous);


            webApp.Uses(searchApi, "Send the Request to get Information/Authenticate", "JSON/Http", InteractionStyle.Asynchronous);
            webApp.Uses(notificationSystem, "Send link Email/SMS notification ", "JSON/Http", InteractionStyle.Asynchronous);
            searchApi.Uses(authenticationService, "Authenticates internal/external users", "RCP/TCP/Sql Server", InteractionStyle.Synchronous);
            searchApi.Uses(elasticSearch, "Searches in the Indexes for the information requested ", "Json/Http", InteractionStyle.Asynchronous);
            dataPopulation.Uses(elasticSearch, "Updates the information in the Indexes", "JSON/HTTP",InteractionStyle.Asynchronous);
            dataPopulation.Uses(debtorMatrix, "Gets information of Cases, Claims, Docket, Key Documents from","RPC / TCP / Sql Server",InteractionStyle.Synchronous);

            dataPopulation.Uses(caseManagement,"Get information of Cases, Claims, Docket,Key Documents (New Information) from", "RPC/TCP/Sql Server",InteractionStyle.Synchronous);
            searchApi.Uses(debtorMatrix, "Gets documents content, and lookup information","RPC/TCP/Sql Server",InteractionStyle.Synchronous);
            realTimeNotifications.Uses(notificationSystem, "Send Email/SMS Notifications Request", "JSON/HTTPS", InteractionStyle.Asynchronous);
            realTimeNotifications.Uses(debtorMatrix, "Gets Docket Changes Events from", "RPC/TCP/Sql Server Service Broker",  InteractionStyle.Asynchronous);
            realTimeNotifications.Uses(notificationRepository, "Get Configuration to Wire Up Real Time Noticing","TCP/SQL", InteractionStyle.Synchronous);

            ContainerView epiq11ContainerView = viewSet.CreateContainerView(epiq11, "Containers", "");
            epiq11ContainerView.PaperSize = PaperSize.A3_Landscape;
            epiq11ContainerView.AddAllElements();


            // WeAbAPi component diagram

            ComponentView searchApiComponentView = viewSet.CreateComponentView(searchApi, "SearchWebApi", "");

            Component searchController = searchApi.AddComponent("ApiControllers", "ApiController","Handles request from WebApp", "WebApi");

            Component commandHandlers = searchApi.AddComponent("Command Handlers", "IRequestHandler","Handles all search command","");

            Component dataRepositories = searchApi.AddComponent("Repositories", "IRepostory", "Allow query in to the Sql Databases", "");

            Component elasticSearchClient = searchApi.AddComponent("Elastic Search Client", "NEST","Provides the connectivity to ElasticSearch", "NEST/ElasticSearch");

            webApp.Uses(searchController, "Send search request to", "JSON/HTTP", InteractionStyle.Asynchronous);
            searchController.Uses(commandHandlers, "Send search command to","", InteractionStyle.Asynchronous);
            commandHandlers.Uses(dataRepositories, "Gets informacion through", "", InteractionStyle.Asynchronous);

            commandHandlers.Uses(elasticSearchClient,"Send command statement, get the result to send to the web application");

            dataRepositories.Uses(debtorMatrix,"Gets documents content, and lookup information", "RPC/TCP/Sql Server", InteractionStyle.Synchronous);

            elasticSearchClient.Uses(elasticSearch, "Searches in the Indexes for the information requested from", "Json/Http", InteractionStyle.Asynchronous);


            searchApiComponentView.PaperSize = PaperSize.A3_Landscape;
            searchApiComponentView.Add(webApp);
            searchApiComponentView.Add(searchController);
            searchApiComponentView.Add(commandHandlers);
            searchApiComponentView.Add(elasticSearchClient);
            searchApiComponentView.Add(dataRepositories);
            searchApiComponentView.Add(debtorMatrix);
            searchApiComponentView.Add(elasticSearch);

            // DataPopulation Component diagram

            ComponentView datapopulationComponentView = viewSet.CreateComponentView(dataPopulation, "DataPopulation","");
            Component populationController = dataPopulation.AddComponent("ApiControllers","WebApi Controller for On-Demand updates", "WebApi 2.1/.Net");
            Component indexerJob = dataPopulation.AddComponent("Quartz Job", "Quartz Job for Schedule data updates","Quartz/.Net");
            Component indexer = dataPopulation.AddComponent("Indexers","Get Transactional Data, apply transformations and push it to ElasticSearch", ".Net 4.5.2");
            Component dataProvider = dataPopulation.AddComponent("DataProviders","Data Access Provider for Sql Transactional Databases", "Ado.Net");
            Component elasticSearchClient2 = dataPopulation.AddComponent("Elastic Search Client", "NEST", "Provides the connectivity to ElasticSearch", "NEST/ElasticSearch");


            dataUser.Uses(populationController, "Triggers a data update into", "Http", InteractionStyle.Asynchronous);
            populationController.Uses(indexer, "Send the request to be processed by", ".Net",InteractionStyle.Asynchronous);
            indexerJob.Uses(indexer, "Send the job to be processed by", ".Net", InteractionStyle.Asynchronous);
            indexer.Uses(dataProvider, "Gets Transactional Data from SQl Databases", "", InteractionStyle.Synchronous);
            indexer.Uses(elasticSearch, "Create/Update documents into ElasticSearch", "", InteractionStyle.Asynchronous);
            dataProvider.Uses(debtorMatrix, "Gets information of Cases, Claims, Docket,Key Documents from","RPC/TCP/Sql Server", InteractionStyle.Synchronous);
            dataProvider.Uses(caseManagement, "Gets information of Cases, Claims, Docket,Key Documents from","RPC/TCP/Sql Server", InteractionStyle.Synchronous);
            elasticSearchClient2.Uses(elasticSearch, "Updates the information in the Indexes", "JSON/HTTP", InteractionStyle.Asynchronous);

            datapopulationComponentView.PaperSize = PaperSize.A3_Landscape;
            datapopulationComponentView.Add(populationController);
            datapopulationComponentView.Add(indexerJob);
            datapopulationComponentView.Add(indexer);
            datapopulationComponentView.Add(dataProvider);
            datapopulationComponentView.Add(elasticSearch);
            datapopulationComponentView.Add(dataUser);
            datapopulationComponentView.Add(debtorMatrix);
            datapopulationComponentView.Add(caseManagement);


            StructurizrClient structurizrClient = new StructurizrClient("233c648d-3e3b-4f41-9461-986edad9ac52", "844548f9-1033-4063-9b23-5864fede0b46");
            structurizrClient.MergeWorkspace(20571, epiq11WorkSpace);
            Console.ReadKey();
        }
    }
}

