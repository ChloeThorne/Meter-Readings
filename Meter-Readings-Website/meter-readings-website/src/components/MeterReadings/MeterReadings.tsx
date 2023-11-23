import React, { FC, useEffect, useState } from 'react';
import { MeterReading } from '../../types/MeterReading';
import {
  Table,
  Thead,
  Tbody,
  Tfoot,
  Tr,
  Th,
  Td,
  TableCaption,
  TableContainer,
  Center,
  Heading,
  Modal,
  ModalOverlay,
  ModalContent,
  useDisclosure,
  Button,
} from '@chakra-ui/react'
import styles from './MeterReadings.module.css';
import MeterReadingUploadResultModal from '../MeterReadingResultModal/MeterReadingUploadResultModal';
import { UploadMeterReadingsViewModel } from '../../types/UploadMeterReadingsViewModel';
import CreateMeterReadingModal from '../CreateMeterReadingModal/CreateMeterReadingModal';

interface MeterReadingsProps { }

const MeterReadings: FC<MeterReadingsProps> = () => {
  const [meterReadings, setMeterReadings] = useState<MeterReading[] | null>();
  const { isOpen: meterReadingUploadResultIsOpen, onOpen: meterReadingUploadResultOnOpen, onClose: meterReadingUploadResultOnClose } = useDisclosure();
  const { isOpen: createMeterReadingIsOpen, onOpen: createMeterReadingOnOpen, onClose: createMeterReadingOnClose } = useDisclosure();
  const [meterReadingUploadResult, setMeterReadingUploadResult] = useState<UploadMeterReadingsViewModel | null>();
  
  function fetchPageData() {
    fetch('https://localhost:7288/MeterReading')
      .then(result => result.json())
      .then(result => {
        const meterReadingResult = result.map((x: any) => {
          return {
            id: x.id,
            accountId: x.accountId,
            meterReadingDateTime: new Date(x.meterReadingDateTime),
            meterReadValue: x.meterReadValue
          }
        })

        setMeterReadings(meterReadingResult);
      });
  }

  const uploadCsv = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const file = e.target.files?.item(0) as Blob;

    const formData = new FormData();
    formData.append("formFile", file);

    fetch('https://localhost:7288/MeterReading/meter-reading-uploads', {
      method: 'post',
      body: formData
    })
      .then(result => result.json())
      .then(result => {
        setMeterReadingUploadResult(result as UploadMeterReadingsViewModel);
        meterReadingUploadResultOnOpen();
        fetchPageData();
        e.target.value = "";
      });
  }

  // On the initial load, get all meter readings.
  useEffect(() => {
    fetchPageData();
  }, []);

  return (
    <div>
      <Center className={styles["meter-reading-header"]}>
        <Heading>Meter Readings</Heading>
      </Center>
      <div className={styles["btn-upload-container"]}>
        <input type="file" accept=".csv" onChange={uploadCsv} />
      </div>
      <Center>
        <TableContainer>
          <Table>
            <Thead>
              <Tr>
                <Th>Id</Th>
                <Th>Account Id</Th>
                <Th>Meter Reading Date</Th>
                <Th>Value</Th>
              </Tr>
            </Thead>
            <Tbody>
              {
                meterReadings?.map(meterReading => (
                  <Tr>
                    <Td>{meterReading.id}</Td>
                    <Td>{meterReading.accountId}</Td>
                    <Td>{meterReading.meterReadingDateTime.toDateString()}</Td>
                    <Td>{meterReading.meterReadValue}</Td>
                  </Tr>
                ))
              }

            </Tbody>
          </Table>
        </TableContainer>
      </Center>
      <div className={styles["btn-create-container"]}>
        <Button onClick={createMeterReadingOnOpen}>
          Create
        </Button>
      </div>
      <MeterReadingUploadResultModal isOpen={meterReadingUploadResultIsOpen} onOpen={meterReadingUploadResultOnOpen} onClose={meterReadingUploadResultOnClose} meterReadingUploadResult={meterReadingUploadResult}></MeterReadingUploadResultModal>
      <CreateMeterReadingModal isOpen={createMeterReadingIsOpen} onOpen={createMeterReadingOnOpen} onClose={createMeterReadingOnClose} reloadPageDate={fetchPageData}></CreateMeterReadingModal>

    </div>
  );
}

export default MeterReadings;
