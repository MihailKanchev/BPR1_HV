import pandas as pd
from sklearn.model_selection import train_test_split
from imblearn.under_sampling import RandomUnderSampler
from sklearn.ensemble import GradientBoostingClassifier
import joblib

df = pd.read_csv('ExodiaEncoded.csv',delimiter=',', header=0, parse_dates=[['date', 'time']], index_col='date_time')

df = df.dropna()

df['month'] = [df.index[i].month for i in range(len(df))]
df['day'] = [df.index[i].day for i in range(len(df))]
df['hour'] = [df.index[i].hour for i in range(len(df))]
df.reset_index(drop=True, inplace=True)

df = df[['P1StartQuantity', 'P2StartQuantity','P1OperatingTime(m)','P2OperatingTime(m)', 'Rain(mm)',
         'Niveau(cm)', 'month', 'day','hour','EventText']]

X = df.iloc[:,:9]
y = df['EventText']

rus = RandomUnderSampler()
X_rus, y_rus = rus.fit_sample(X, y)

X_train, X_test, y_train, y_test = train_test_split(X_rus, y_rus, stratify=y_rus, test_size=0.3)

gbrt = GradientBoostingClassifier(max_depth=4, n_estimators=100, learning_rate=.1,random_state=42)
gbrt.fit(X_train,y_train)

joblib.dump(gbrt, 'model.pkl')