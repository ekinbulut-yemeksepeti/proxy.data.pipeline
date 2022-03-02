# Proxy Data Pipeline

A generic abstract layer to implement MongoDb change streams. 


* ```ChangeStreamService``` is a generic abstract class which creates a mongo change stream of the given document.
* Pipeline includes it's own MongoContext
* ```IDocument``` is a interface which should be implemented to derivatives of a document.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)