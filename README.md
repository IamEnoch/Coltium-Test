# Email Service Configuration

To configure the email service in your application, you need to set up the necessary settings in either the `appsettings.json` or `secrets.json` file.

## Configuration

### appsettings.json

Add the following section to your `appsettings.json` file:

```json
{
  "Mailgun": {
    "ApiKey": "your-mailgun-api-key",
    "Domain": "your-mailgun-domain"
  }
}
```
OR

### secrets.json
 
Add the following section to your `secrets.json` file:

```json
{
  "Mailgun": {
    "ApiKey": "your-mailgun-api-key",
    "Domain": "your-mailgun-domain"
  }
}
```