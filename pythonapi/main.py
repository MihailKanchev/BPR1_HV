from flask import Flask
from flask_restful import Api, Resource, request
import joblib
import numpy as np
import json

app = Flask(__name__)
api = Api(app)
model = joblib.load('model.pkl')




class Forecast(Resource):  
    def put(self):
        print(request.get_data())
        raw_data = request.get_data()
        
        args = json.loads(raw_data)
        
        to_list = list(args.values())
        nparray = np.array(to_list)
        
        prediction = model.predict(nparray.reshape(1, -1))
        proba = model.predict_proba(nparray.reshape(1, -1))
        
        answer = prediction+',' + str(proba[0])
        
        return  json.dumps(answer.tolist()), 201
     
    
api.add_resource(Forecast, "/")
if __name__ == "__main__":
    app.run(debug=True)
