# online-availability-monitor
Small cloud service to regularly check for availability of configured web content, alert in case of instability and send regular reports.

1. Compilation
Use Visual Studio Code to compile the project.

2. Configuration
Service uses two configurations. Local (per service instance) and Centralized (singleton remote). Local config lives in the config.json file and points to the remote config location (DB connection string). Remove config lives in table called Config in a mysql database.

3. Execution
Run service in as many regions as desired. Make sure you place the correct "region" attribute in config.json. Service wakes up, reads local config and learns which region is it running and where is the database. Service reads remove config from database and starts pinging configure URLs.

4. Results
During its execution, service pushes ping stats back to the database (HTTP result code, latency, ...).
