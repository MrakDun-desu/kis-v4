import * as api from './api-generated'
import './App.css'
import { Button, TextField } from '@mui/material'
import { useForm } from 'react-hook-form'

const jwt_key = "user_jwt"

const apiConf = new api.Configuration({
  basePath: import.meta.env.VITE_API_URL ?? api.BASE_PATH,
  accessToken: () => localStorage.getItem(jwt_key) ?? ""
})

let storeItemsApi = new api.StoreItemsApi(apiConf)

interface TestingForm {
  jwt: string
  basePath: string
}

function App() {
  const { register, handleSubmit } = useForm<TestingForm>();

  const login = (data: TestingForm) => {
    console.log(data);
    const conf = new api.Configuration({
      basePath: data.basePath,
      accessToken: data.jwt
    })
    storeItemsApi = new api.StoreItemsApi(conf);
  }

  const queryStoreItems = async () => {
    console.log(await storeItemsApi.storeItemsGet());
  }

  return (
    <>
      <h1>KISv4 testing</h1>
      {
        import.meta.env.DEV &&
        <>
          <form onSubmit={handleSubmit(login)}>
            <TextField label="Testing JWT" {...register("jwt")} />
            <TextField label="Base path" {...register("basePath")} />
            <Button type="submit">Login to API</Button>
          </form>

          <Button type="button" onClick={queryStoreItems}>Query store items</Button>
        </>
      }
    </>
  )
}

export default App
