import React, { FC, useState } from 'react';
import styles from './CreateMeterReadingModal.module.css';
import { Button, Center, FormControl, FormLabel, Input, Modal, ModalBody, ModalCloseButton, ModalContent, ModalHeader, ModalOverlay, NumberInput, NumberInputField, useToast } from '@chakra-ui/react';
import { MeterReading } from '../../types/MeterReading';
import { CreateMeterReadingViewModel } from '../../types/CreateMeterReadingViewModel';

interface CreateMeterReadingModalProps { isOpen: boolean, onClose: () => void, onOpen: () => void, reloadPageDate: () => void }

const CreateMeterReadingModal: FC<CreateMeterReadingModalProps> = (props) => {
  const [meterReading, setMeterReading] = useState<CreateMeterReadingViewModel>({
    accountId: 0,
    meterReadingDateTime: new Date(),
    meterReadValue: 0
  });

  const toast = useToast();

  async function createMeterReading(e: any) {
    e.preventDefault();

    fetch('https://localhost:7288/MeterReading', {
      method: 'post',
      body: JSON.stringify(meterReading),
      headers: {
        "Content-Type": "application/json"
      }
    }).then(result => {
      if (result.status == 204) {
        props.onClose();
        props.reloadPageDate();

        toast({
          title: 'Meter reading created.',
          position: "top-right",
          status: 'success',
          duration: 9000,
          isClosable: true,
        })
      }
      else {
        toast({
          title: 'Creation failed.',
          position: "top-right",
          description: "Something went wrong, please try again.",
          status: 'error',
          duration: 9000,
          isClosable: true,
        })
      }
    })
  }

  return (
    <div className={styles.CreateMeterReadingModal}>
      <Modal isOpen={props.isOpen} onClose={props.onClose}>
        <ModalOverlay />
        <ModalContent>
          <ModalHeader>
            <Center>
              Create Meter Reading
            </Center>
          </ModalHeader>
          <ModalCloseButton />
          <ModalBody>
            <form onSubmit={createMeterReading}>
              <FormControl className={styles["form-control"]}>
                <FormLabel>Account Id</FormLabel>
                <NumberInput>
                  <NumberInputField value={meterReading.accountId} onChange={(e) => setMeterReading({ ...meterReading, accountId: parseInt(e.target.value) })}></NumberInputField>
                </NumberInput>
              </FormControl>
              <FormControl className={styles["form-control"]}>
                <FormLabel>Meter Reading Date Time</FormLabel>
                <Input type='datetime-local' onChange={(e) => setMeterReading({ ...meterReading, meterReadingDateTime: new Date(e.target.value) })} />
              </FormControl>
              <FormControl className={styles["form-control"]}>
                <FormLabel>Meter Read Value</FormLabel>
                <NumberInput>
                  <NumberInputField value={meterReading.meterReadValue} onChange={(e) => setMeterReading({ ...meterReading, meterReadValue: parseInt(e.target.value) })} ></NumberInputField>
                </NumberInput>
              </FormControl>
              <Center>
                <Button className={styles["form-control"]} type="submit">Create</Button>
              </Center>
            </form>
          </ModalBody>
        </ModalContent>
      </Modal>
    </div>
  );
}


export default CreateMeterReadingModal;
