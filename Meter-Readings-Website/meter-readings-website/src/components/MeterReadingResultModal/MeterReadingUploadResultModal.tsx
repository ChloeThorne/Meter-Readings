import React, { FC } from 'react';
import styles from './MeterReadingUploadResultModal.module.css';
import { Modal, ModalBody, ModalCloseButton, ModalContent, ModalHeader, ModalOverlay, Text, Center } from '@chakra-ui/react';
import { UploadMeterReadingsViewModel } from '../../types/UploadMeterReadingsViewModel';

interface MeterReadingUploadResultModalProps {isOpen: boolean, onClose: () => void, onOpen: () => void, meterReadingUploadResult: UploadMeterReadingsViewModel | null | undefined}

const MeterReadingUploadResultModal: FC<MeterReadingUploadResultModalProps> = (props) => {
  return (
    <div className={styles.MeterReadingResultModal}>
        <Modal isOpen={props.isOpen} onClose={props.onClose}>
          <ModalOverlay />
          <ModalContent>
            <ModalHeader>
              <Center>
                Meter Readings Uploaded
              </Center>
            </ModalHeader>
            <ModalCloseButton />
            <ModalBody>
              <Center>
                <Text>
                  {props.meterReadingUploadResult?.successfulReadingsCount} were successfully uploaded.
                </Text>
              </Center>
              <Center>
                <Text>
                  {props.meterReadingUploadResult?.failedReadingsCount} were not uploaded.
                </Text>
              </Center>
          </ModalBody>
          </ModalContent>
        </Modal>
    </div>
  );
} 

export default MeterReadingUploadResultModal;
