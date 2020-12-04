from flask import Flask
from flask_restful import Api, Resource, reqparse
import random

app = Flask(__name__)
api = Api(app)

forecast_put_args = reqparse.RequestParser()
forecast_put_args.add_argument("P1StartQuantity", type=float, help="Start quantity of pump 1.")
forecast_put_args.add_argument("P1OperatingTime(m)", type=float, help="Operating time of pump 1.")
forecast_put_args.add_argument("P2StartQuantity", type=float, help="Start quantity of pump 2.")
forecast_put_args.add_argument("P2OperatingTime(m)", type=float, help="Operating time of pump 2.")
forecast_put_args.add_argument("Rain(mm)", type=float, help="Rain in mm")
forecast_put_args.add_argument("Niveau(cm)", type=float, help="Level of water inside in cm.")
forecast_put_args.add_argument("Month", type=int, help="Number of month of the reading.")
forecast_put_args.add_argument("Day", type=int, help="Number of day of the reading.")
forecast_put_args.add_argument("Hour", type=int, help="Hour of the reading (0-23).")


class Forecast(Resource):  
    def put(self):
        args = forecast_put_args.parse_args()
        response = random.choice( ['System operates under norms.', 'The system needs supervision!'] )
        return response , 201
    
    
api.add_resource(Forecast, "/")
if __name__ == "__main__":
    app.run(debug=True)
