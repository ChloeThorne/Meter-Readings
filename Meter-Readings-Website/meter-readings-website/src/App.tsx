import { ChakraProvider } from "@chakra-ui/react";
import MeterReadings from "./components/MeterReadings/MeterReadings";

export default function App() {
    return (
        <ChakraProvider>
            <MeterReadings />
        </ChakraProvider>
    )
}